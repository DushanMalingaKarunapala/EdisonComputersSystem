using BLL.Services.JobService;
using Core.Enums;
using Core.Models;
using Core.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Drawing;
using QuestPDF.Elements;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Net.Mail;

namespace Presentation.SubSections.Jobs
{
    public partial class InvoiceDialog : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly int _jobId;
        private Job _loadedJobWithDetails; // To store all fetched data
        private Customer _customerDetails; // To store customer contact info
                                           // NEW: Add a flag to control stamp/signature inclusion
        private readonly bool _includeStampsAndSignatures; // Set to true to include, false to exclude

        // --- NEW: Member variables to hold PDF data and temp file path ---
        private byte[] _pdfBytes;
        private string _tempPdfPath;


        string shopEmail = ConfigurationManager.AppSettings["ShopEmail"];
        string shopPassword = ConfigurationManager.AppSettings["ShopEmailPassword"];
        string fontPath = ConfigurationManager.AppSettings["FontPath"];

        string imagePath = ConfigurationManager.AppSettings["imagePath"];
        string watermarkPath = ConfigurationManager.AppSettings["watermarkpath"];
        string smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
        int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
        string whatsAppBaseUrl = ConfigurationManager.AppSettings["WhatsAppBaseUrl"];
        string invoicePdfTempPattern = ConfigurationManager.AppSettings["InvoicePdfTempPattern"];
        string invoicePdfDocPattern = ConfigurationManager.AppSettings["InvoicePdfDocPattern"];
        string companyName = ConfigurationManager.AppSettings["CompanyName"];
        string companySlogan = ConfigurationManager.AppSettings["CompanySlogan"];
        string companyAddress = ConfigurationManager.AppSettings["CompanyAddress"];
        string companyPhone = ConfigurationManager.AppSettings["CompanyPhone"];
        string companyEmail = ConfigurationManager.AppSettings["CompanyEmail"];
        string companyFacebook = ConfigurationManager.AppSettings["CompanyFacebook"];



        public InvoiceDialog(IServiceProvider serviceProvider, int jobId, bool includeStampsAndSignatures = true)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _jobId = jobId;
            _includeStampsAndSignatures = includeStampsAndSignatures; // Set the flag based on constructor parameter

            // --- REGISTER THE FONT ---
            // This only needs to be done once, but doing it here is safe.
            try
            {
                // The font file should be in a 'Fonts' sub-folder of your running application
                //string fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Fonts", "Iskoola Pota Regular.ttf");
                if (File.Exists(fontPath))
                {
                    FontManager.RegisterFont(File.OpenRead(fontPath));
                }
                else
                {
                    // Handle case where font is missing, maybe log a warning.
                    Debug.WriteLine("WARNING: Sinhala font 'IskoolaPota.ttf' not found.");
                }
            }
            catch (Exception ex)
            {
                // Log any exception during font registration
                Debug.WriteLine($"Error registering font: {ex.Message}");
            }





            // --- NEW: Add a closing event to clean up the temp file ---
            this.FormClosing += InvoiceDialog_FormClosing;


        }


        private async void InvoiceDialog_Load(object sender, EventArgs e)
        {
            //await LoadInvoiceDataAsync(); // Load job and customer data asynchronously
            await LoadAndPreviewInvoiceAsync(); // Load data and generate PDF for preview
        }


        private async Task LoadAndPreviewInvoiceAsync()
        {
            try
            {
                // 1. Load the data from the database
                bool dataLoaded = await LoadInvoiceDataAsync();
                if (!dataLoaded) return; // Stop if data loading failed

                // 2. Generate the PDF bytes
                _pdfBytes = GenerateInvoicePdf();
                if (_pdfBytes == null || _pdfBytes.Length == 0)
                {
                    MessageBox.Show("Failed to generate PDF data for preview.", "Error");
                    return;
                }

                // 3. Save to a temporary file and display in WebView2
                //_tempPdfPath = Path.Combine(Path.GetTempPath(), $"invoice_{_jobId}_{Guid.NewGuid()}.pdf");
                _tempPdfPath = Path.Combine(Path.GetTempPath(), string.Format(invoicePdfTempPattern, _jobId, Guid.NewGuid()));
                await File.WriteAllBytesAsync(_tempPdfPath, _pdfBytes);

                // Initialize WebView2 and navigate to the temp PDF file
                await webViewPriview.EnsureCoreWebView2Async(null);
                webViewPriview.CoreWebView2.Navigate($"file:///{_tempPdfPath}");

                // Enable the action buttons now that the preview is ready
                btnPrint.Enabled = true;
                btndownloadPdfDoc.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while preparing the invoice preview: {ex.Message}", "Preview Error");
                this.Close();
            }
        }



        private async Task<bool> LoadInvoiceDataAsync()
        {

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var jobService = scope.ServiceProvider.GetRequiredService<JobService>();
                    _loadedJobWithDetails = await jobService.GetJob(_jobId);

                    if (_loadedJobWithDetails == null || _loadedJobWithDetails.Device == null || _loadedJobWithDetails.Device.Customer == null || _loadedJobWithDetails.Billing == null)
                    {
                        MessageBox.Show("Could not load complete job details for invoice.", "Error");
                        this.Close();
                        return false;
                    }
                    _customerDetails = _loadedJobWithDetails.Device.Customer;
                    CheckCommunicationOptions();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading invoice data: {ex.Message}", "Error");
                this.Close();
                return false;
            }
        }

       

        // Optional: Method to check communication options based on customer data
        private void CheckCommunicationOptions()
        {
            // Enable/disable email and SMS buttons based on customer contact info
            btnsendEmail.Enabled = !string.IsNullOrEmpty(_customerDetails?.Email);
            btnsendSMS.Enabled = !string.IsNullOrEmpty(_customerDetails?.Phone);
        }




        //warranty expiry display logic based on job status and collection date
        //even tho warrantyperiodinmonths is called warrantyperiodinmonths, it can be in days or months based on the WarrantyUnit
        private static string GetWarrantyDisplayString(DateTime? expiryDate, int? warrantyPeriodInMonths, WarrantyUnit? warrantyUnit)
        {
            //// If warranty has an active start date and thus an expiry date
            //if (expiryDate.HasValue)
            //{
            //    return $"Warranty until: {expiryDate.Value:MM/dd/yyyy}";
            //}
            //// If warranty has a duration but hasn't started yet
            //else if (warrantyPeriodInMonths.HasValue && warrantyPeriodInMonths > 0)
            //{
            //    //return $"Warranty: {warrantyPeriodInMonths.Value} months from pickup";

            //    string unitString = warrantyUnit == WarrantyUnit.Days ? "Days" : "Months";
            //    return $"Warranty: {warrantyPeriodInMonths.Value} {unitString} from pickup";
            //}
            //// No warranty
            //else
            //{
            //     return $"No Warranty";
            //}

            // For invoice: ONLY show warranty period, never the end date
            if (warrantyPeriodInMonths.HasValue && warrantyPeriodInMonths > 0)
            {
                string unitString = warrantyUnit == WarrantyUnit.Days ? "Days" : "Months";
                return $"{warrantyPeriodInMonths.Value} {unitString}";
            }
            else
            {
                return "N/A";
            }
        }









        //-------------------invoice header and footer generation-------------------//

        // Method to compose the header of the PDF document
        private void ComposeHeader(IContainer container)
        {
            //get the logo image path from the project resources or a specific path
            // Build full paths from relative config values
            string imagePath = ConfigurationManager.AppSettings["imagePath"];
            string fullImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);

            string phoneIconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["phoneIconPath"]);
            string emailIconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["emailIconPath"]);
            string locationIconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["locationIconPath"]);
            string facebookIconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["facebookIconPath"]);

            // Check if files exist before loading
            if (!File.Exists(fullImagePath))
            {
                Debug.WriteLine($"Image not found: {fullImagePath}");
                return; // or use a default image
            }

            byte[] imageBytes = File.ReadAllBytes(fullImagePath); // Load the image bytes
            byte[] phoneIconBytes = File.ReadAllBytes(phoneIconPath); // Load phone icon bytes
            byte[] emailIconBytes = File.ReadAllBytes(emailIconPath); // Load email icon bytes
            byte[] locationIconBytes = File.ReadAllBytes(locationIconPath); // Load location icon bytes
            byte[] facebookIconBytes = File.ReadAllBytes(facebookIconPath); // Load Facebook icon bytes


            // --- Step 2: Define Content ---
            var services = new[]
                    {
                "Laptop & Desktop Repair",
                "Graphics Card Repair",
                "OS Installation",
                "Accessories",
                "PC Builds",
                "PC Service"
                    };

            // --- Step 3: Build the Header Layout ---
            container
        .ExtendHorizontal()                 // Stretches to full page width
        .Background(Colors.Grey.Lighten4)   // Applies the background color
        .PaddingVertical(5)                // Adds vertical spacing
        .PaddingHorizontal(0.5f, Unit.Centimetre) // Adds horizontal spacing from page edge
        .Column(mainColumn =>
            {
                // This is the main vertical column for the header content
                mainColumn.Spacing(8); // This adds space between the top section and the services section

                // --- Section A: Top Row with Logo/Name (Left) and Contact Details (Right) ---
                mainColumn.Item().Row(topRow =>
                {
                    // Left side: This will contain the logo and company name. It's a "RelativeItem"
                    // so it will take up the remaining space.
                    topRow.RelativeItem().Row(logoAndNameRow =>
                    {
                        logoAndNameRow.Spacing(8); // Space between logo and text

                        // Logo container with a fixed size
                        logoAndNameRow.ConstantItem(120).PaddingTop(2).Image(imageBytes);

                        // Name and Slogan container
                        logoAndNameRow.RelativeItem().Column(nameCol =>
                        {
                            // Padding to help vertically align text with the middle of the logo
                            nameCol.Item().PaddingTop(6).Text("Edison Computers")
                                .Bold().FontSize(24).FontColor(Colors.Red.Darken2);

                            nameCol.Item().Text("Illuminating Your Digital World")
                                .SemiBold().FontSize(12).FontColor(Colors.Grey.Darken2);
                        });
                    });

                    // Right side: This contains the contact details. It's a "ConstantItem"
                    // with a fixed width, which is a safe way to create columns.
                    // Right side: Contact details with icons
                    topRow.ConstantItem(250).AlignRight().PaddingTop(5).PaddingRight(1).Column(contactCol => // Increased width slightly for icons
                    {
                        contactCol.Spacing(4);

                        // --- NEW: Helper Action to create a line with Text and an Icon ---
                        Action<string, byte[]> contactLine = (text, iconBytes) =>
                        {
                            contactCol.Item().Row(lineRow =>
                            {
                                // Text is aligned to the right within its available space
                                lineRow.RelativeItem().AlignRight().Text(text).FontSize(9);

                                // Icon is in a fixed-size container to the right of the text
                                lineRow.ConstantItem(20).PaddingLeft(5).Image(iconBytes);
                            });
                        };

                        // --- Use the helper to build the contact section ---
                        contactLine(companyAddress, locationIconBytes);
                        contactLine(companyPhone, phoneIconBytes);
                        contactLine(companyEmail, emailIconBytes);
                        contactLine(companyFacebook, facebookIconBytes);
                    });
                });

                // --- Section B: Services Offered, Framed by Lines ---
                // This is the section you wanted to add back. It is a new item in the mainColumn.
                mainColumn.Item().Column(servicesCol =>
                {
                    servicesCol.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium); // Top line

                    servicesCol.Item()
                        .PaddingVertical(5) // Add space around the text
                        .AlignCenter()      // Center the services text
                        .Text(string.Join("  |  ", services)) // Join services with a separator
                        .FontSize(9).Bold().FontColor(Colors.Grey.Darken2);

                    servicesCol.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium); // Bottom line
                });
            });
        }

        //                    row.RelativeItem().Text("INVOICE - Edison Computers").SemiBold().FontSize(30).FontColor(Colors.Red.Medium);




        // ---- START: New Refactored Code for page.Content() ----

        // Helper method for the new two-column layout
        private void ComposeCustomerAndInvoiceDetails(IContainer container)
        {
            container
            .Border(1).BorderColor(Colors.Grey.Lighten2) // Single border around the whole section
            .Row(row =>
            {
                // --- Column 1: Customer Details ---
                row.RelativeItem().Padding(8).Column(col =>
                {
                    col.Spacing(4);
                    col.Item().Text("Customer Details").SemiBold().FontSize(10);
                    col.Item().LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);
                    col.Item().Text($"Name : { _customerDetails.FirstName + " " + _customerDetails.LastName}");
                    //if the job upgrade record type is newpurchase or completepcbuild dont show the device information because it is not related to any device
                    if (_loadedJobWithDetails?.UpgradeRecords != null && _loadedJobWithDetails.UpgradeRecords.Any(ur => ur.UpgradeType == UpgradeType.NewPurchase || ur.UpgradeType == UpgradeType.CompletePcBuild))
                    {
                        col.Item().Text("Device : -");
                    }
                    else

                        col.Item().Text($"Device : {_loadedJobWithDetails?.Device?.Brand} {_loadedJobWithDetails?.Device?.Model} (SN: {_loadedJobWithDetails?.Device?.SerialNumber ?? "N/A"})");
                    col.Item().Text($"Phone : {_customerDetails.Phone ?? "N/A"}");
                });

                // A vertical line separating the columns
                row.ConstantItem(1).Background(Colors.Grey.Lighten2);

                // --- Column 2: Invoice Details ---
                row.RelativeItem().Padding(8).Column(col =>
                {
                    col.Spacing(4);
                    // Helper to create label: value pairs
                    Action<string, string, string?> detailRow = (label, value, color) =>
                    {
                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Text(label).FontSize(8);
                            var valueText = r.RelativeItem().AlignRight().Text(value).SemiBold();
                            if (!string.IsNullOrEmpty(color))
                                valueText.FontColor(color);
                        });
                    };

                    detailRow("Invoice No:", $"INV-{_loadedJobWithDetails?.Billing.BillingId:D5}",null);
                    detailRow("Job ID:", _jobId.ToString(),null);
                    detailRow("Date:", DateTime.Now.ToString("yyyy-MM-dd"), null);
                    detailRow("Payment Status:",
                        _loadedJobWithDetails?.Billing?.IsPaid == true
                            ? $"PAID on {_loadedJobWithDetails.Billing.PaymentDate?.ToString("yyyy-MM-dd h:mm tt", System.Globalization.CultureInfo.InvariantCulture)} via {_loadedJobWithDetails.Billing.PaymentMethod}"
                            : "UNPAID",
                        _loadedJobWithDetails?.Billing?.IsPaid == true ? Colors.Green.Medium : Colors.Red.Medium
                    );

                });
            });
        }



        // Helper method for the device information
        //private void ComposeDeviceDetails(IContainer container)
        //{
        //    container.Column(column =>
        //    {
        //        column.Spacing(5);
        //        column.Item().Text("Device Information").SemiBold();
        //        column.Item().Text($"{_loadedJobWithDetails?.Device?.Brand} {_loadedJobWithDetails?.Device?.Model} (SN: {_loadedJobWithDetails?.Device?.SerialNumber ?? "N/A"})");
        //    });
        //}





        // Helper method for the line items table
        private void ComposeLineItemsTable(IContainer container)
        {   
            
            container.MinHeight(350) // Ensure the table has enough space so the bottom section stays at the bottom without jumping around



            .Table(table =>
            {
                // --- Step 1: Define Columns ---
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(4);  // Description
                    columns.RelativeColumn(2);  // Warranty
                    columns.ConstantColumn(25); // Qty
                    columns.RelativeColumn(1.5f); // Unit Price
                    columns.RelativeColumn(1.5f); // Amount
                });

                // --- Step 2: Define Header ---
                table.Header(header =>
                {
                    header.Cell().Background(Colors.Grey.Lighten3).Padding(4).Text("Description").SemiBold();
                    header.Cell().Background(Colors.Grey.Lighten3).Padding(4).Text("Warranty").SemiBold();
                    header.Cell().Background(Colors.Grey.Lighten3).Padding(4).AlignCenter().Text("Qty").SemiBold();
                    header.Cell().Background(Colors.Grey.Lighten3).Padding(4).AlignRight().Text("Unit Price").SemiBold();
                    header.Cell().Background(Colors.Grey.Lighten3).Padding(4).AlignRight().Text("Amount").SemiBold();
                });

                // --- Step 3: Populate Rows ---

                // --- Loop through Repair Records with Corrected Logic  ---
                foreach (var rr in _loadedJobWithDetails?.RepairRecords ?? Enumerable.Empty<RepairRecord>())
                {
                    // Group Header for the repair task (spans all 5 columns)
                    table.Cell().ColumnSpan(5).Background(Colors.Grey.Lighten4).Padding(4)
                        .Text(rr.ProblemDescription ?? "Repair Task").SemiBold();

                    // Get the warranty text from the PARENT RepairRecord.
                    string repairWarrantyText = GetWarrantyDisplayString(rr.WarrantyExpiryDate, rr.WarrantyPeriodinMonths, rr.WarrantyUnit);

                    // --- RE-INTEGRATED IF/ELSE BLOCK ---
                    if (rr.RepairRecordComponents != null && rr.RepairRecordComponents.Any())
                    {
                        // This repair HAS components. Loop through them.
                        foreach (var rrc in rr.RepairRecordComponents)
                        {
                            // Cell 1: Description (indented)
                            table.Cell().PaddingLeft(10).Padding(2).Column(col =>
                            {
                                col.Item().Text(rrc.Component?.Name ?? "N/A");
                                if (!string.IsNullOrWhiteSpace(rrc.Component?.SerialNumber))
                                {
                                    col.Item().Text($"SN: {rrc.Component.SerialNumber}").FontSize(8).FontColor(Colors.Grey.Darken1);
                                }
                            });

                            // Cell 2: Warranty (from the parent record)
                            table.Cell().Padding(2).Text(repairWarrantyText);

                            // Cells 3, 4, 5: Qty, Price, Amount for the component
                            table.Cell().Padding(2).AlignCenter().Text(rrc.QuantityUsed.ToString());
                            table.Cell().Padding(2).AlignRight().Text(rrc.PricePerUnit.ToString("N2"));
                            table.Cell().Padding(2).AlignRight().Text((rrc.QuantityUsed * rrc.PricePerUnit).ToString("N2"));
                        }
                    }
                    else
                    {
                        // This is a service-only repair with NO components.
                        // Display one line representing the service.

                        // Repair WITHOUT components - show service charge only
                        table.Cell().Padding(2).Text("Repair Service").Italic();
                        table.Cell().Padding(2).Text(repairWarrantyText);
                        table.Cell().Padding(2).AlignCenter().Text("1");
                        table.Cell().Padding(2).AlignRight().Text(rr.ServiceCharge.ToString("N2"));
                        table.Cell().Padding(2).AlignRight().Text(rr.ServiceCharge.ToString("N2"));
                    }
                }

                // --- Loop through Upgrade Records ---
                // (Logic for this remains correct as upgrades are component-based)
                foreach (var ur in _loadedJobWithDetails?.UpgradeRecords ?? Enumerable.Empty<UpgradeRecord>())
                {
                    table.Cell().ColumnSpan(5).Background(Colors.Grey.Lighten4).Padding(4)
                        .Text($"{ur.UpgradeType} ").SemiBold();

                    if (ur.UpgradeRecordComponents != null && ur.UpgradeRecordComponents.Any())
                    {
                        // Upgrade HAS components
                        foreach (var urc in ur.UpgradeRecordComponents)
                        {
                            table.Cell().PaddingLeft(10).Padding(2).Column(col => {
                                col.Item().Text(urc.Component?.Name ?? "N/A");
                                if (!string.IsNullOrWhiteSpace(urc.Component?.SerialNumber))
                                {
                                    col.Item().Text($"SN: {urc.Component.SerialNumber}").FontSize(8).FontColor(Colors.Grey.Darken1);
                                }
                            });

                            table.Cell().Padding(2).Text(GetWarrantyDisplayString(urc.WarrantyExpiryDate, urc.WarrantyPeriodInMonths, null));
                            table.Cell().Padding(2).AlignCenter().Text(urc.QuantityUsed.ToString());
                            table.Cell().Padding(2).AlignRight().Text(urc.PricePerUnit.ToString("N2"));
                            table.Cell().Padding(2).AlignRight().Text((urc.QuantityUsed * urc.PricePerUnit).ToString("N2"));
                        }
                    }
                    else
                    {
                        // Upgrade WITHOUT components - show service charge only
                        table.Cell().Padding(2).Text("Upgrade Service").Italic();
                        table.Cell().Padding(2).Text("N/A");
                        table.Cell().Padding(2).AlignCenter().Text("1");
                        table.Cell().Padding(2).AlignRight().Text(ur.ServiceCharge.ToString("N2"));
                        table.Cell().Padding(2).AlignRight().Text(ur.ServiceCharge.ToString("N2"));
                    }
                }

                // --- Loop through Service Records ---
                foreach (var sr in _loadedJobWithDetails?.ServiceRecords ?? Enumerable.Empty<ServiceRecord>())
                {

                    // Group Header for the repair task (spans all 5 columns)
                    table.Cell().ColumnSpan(5).Background(Colors.Grey.Lighten4).Padding(4)
                        .Text($"{sr.ServiceType}").SemiBold();


                    table.Cell().Padding(2).Text($"Service: {sr.ServiceDescription ?? "-"}");
                    table.Cell().Padding(2).Text("N/A");
                    table.Cell().Padding(2).AlignCenter().Text("1");
                    table.Cell().Padding(2).AlignRight().Text(sr.ServiceCharge.ToString("N2"));
                    table.Cell().Padding(2).AlignRight().Text(sr.ServiceCharge.ToString("N2"));
                }

                // --- Step 4: Totals Section ---
                //table.Cell().ColumnSpan(3); // Empty cells to push totals to the right
                //table.Cell().BorderTop(1).Padding(2).AlignRight().Text("Labor / Service Charges:").SemiBold();
                //table.Cell().BorderTop(1).Padding(2).AlignRight().Text(_loadedJobWithDetails?.ManualJobCost?.ToString("C2") ?? "0.00").SemiBold();

                //table.Cell().ColumnSpan(3);
                //table.Cell().Background(Colors.Grey.Lighten3).Padding(2).AlignRight().Text("GRAND TOTAL:").Bold().FontSize(12);
                //table.Cell().Background(Colors.Grey.Lighten3).Padding(2).AlignRight().Text(_loadedJobWithDetails?.Billing?.Amount.ToString("C2")).Bold().FontSize(12);

                // --- Calculate Totals ---
                decimal totalServiceCharges =
                    (_loadedJobWithDetails?.ServiceRecords?.Sum(sr => sr.ServiceCharge) ?? 0) +
                    (_loadedJobWithDetails?.RepairRecords?.Sum(rr => rr.ServiceCharge) ?? 0) +
                    (_loadedJobWithDetails?.UpgradeRecords?.Sum(ur => ur.ServiceCharge) ?? 0);

                decimal totalComponentCharges =
                    (_loadedJobWithDetails?.RepairRecords?.Sum(rr =>
                        rr.RepairRecordComponents?.Sum(rrc => rrc.QuantityUsed * rrc.PricePerUnit) ?? 0) ?? 0) +
                    (_loadedJobWithDetails?.UpgradeRecords?.Sum(ur =>
                        ur.UpgradeRecordComponents?.Sum(urc => urc.QuantityUsed * urc.PricePerUnit) ?? 0) ?? 0);

                decimal manualLaborCost = _loadedJobWithDetails?.ManualJobCost ?? 0;
                decimal grandTotal = totalServiceCharges + totalComponentCharges + manualLaborCost;

                // --- Display Totals ---
                //table.Cell().ColumnSpan(3);
                //table.Cell().BorderTop(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(2).AlignRight().Text("Service Charges:").SemiBold();
                //table.Cell().BorderTop(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(2).AlignRight().Text(totalServiceCharges.ToString("C2")).SemiBold();

                //table.Cell().ColumnSpan(3);
                //table.Cell().BorderTop(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(2).AlignRight().Text("Component Charges:").SemiBold();
                //table.Cell().BorderTop(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(2).AlignRight().Text(totalComponentCharges.ToString("C2")).SemiBold();

                table.Cell().ColumnSpan(3);
                table.Cell().BorderTop(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(2).AlignRight().Text("Labor Charges:").SemiBold();
                table.Cell().BorderTop(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(2).AlignRight().Text(manualLaborCost.ToString("C2")).SemiBold();

                table.Cell().ColumnSpan(3);
                table.Cell().Background(Colors.Grey.Lighten3).Padding(2).AlignRight().Text("GRAND TOTAL:").Bold().FontSize(12);
                table.Cell().Background(Colors.Grey.Lighten3).Padding(2).AlignRight().Text(grandTotal.ToString("C2")).Bold().FontSize(12);
            });
        }




        // A new helper component to create perfectly aligned bullet points
        private void BulletPoint(IContainer container, string text)
        {
            container.Row(row =>
            {
                // Use a constant item for the bullet for consistent spacing
                row.ConstantItem(15).Text("• ").SemiBold();

                // Use a relative item for the text so it can wrap naturally
                row.RelativeItem().Text(text).FontSize(7);
            });
        }





        // Helper method for the Terms & Conditions and Warranty sections
        private void ComposeTermsAndWarranty(IContainer container)
        {
            // Add some space above this section to separate it from the main content
            container.ShowEntire().MinHeight(180).PaddingTop(2).Column(mainColumn =>
            {
                // A subtle line to visually separate the fine print
                mainColumn.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingBottom(8);

                mainColumn.Item().Row(row =>
                {
                    // Spacing between the two columns
                    row.Spacing(20);

                    // --- Column 1: Terms & Conditions ---
                    row.RelativeItem().Column(column =>
                    {
                        column.Spacing(5); // Vertical spacing between bullet points

                        // Section Title
                        column.Item().Text("📄 නියමයන් සහ කොන්දේසි").SemiBold().FontSize(8);

                        // List of terms using the new BulletPoint component
                        //column.Item().Element(c => BulletPoint(c, "All prices are inclusive/exclusive of applicable taxes (as specified)."));
                        column.Item().Element(c => BulletPoint(c, "වගකීම් කාලය යටතේ ආවරණය වන තහවුරු කරන ලද දෝෂ සහිත අවස්ථාවකදී හැර, වරක් විකුණන ලද භාණ්ඩ ආපසු ලබා දීමට හෝ හුවමාරු කිරීමට සුදුසුකම් නොලබයි."));
                        //column.Item().Element(c => BulletPoint(c, "ලිඛිතව වෙනත් ආකාරයකින් එකඟ වී නොමැති නම්, භාණ්ඩ ලබා ගැනීමේදී ගෙවීම සිදු කළ යුතුය."));
                        column.Item().Element(c => BulletPoint(c, "සම්පූර්ණ වගකීම් ප්‍රතිලාභ ලබාගැනිම සඳහා අලුත්වැඩියාව අවසන් වී දින 7ක් ඇතුළත උපාංග ලබාගත යුතුය."));
                        column.Item().Element(c => BulletPoint(c, "භාණ්ඩය බෙදා හැරීමෙන් හෝ ලබා ගැනීමෙන් පසු, යම් ගැටළුවක් ඇත්නම් ඒ බව දින 3ක් ඇතුළත දැනුවත් කළ යුතුය."));
                        column.Item().Element(c => BulletPoint(c, "භාණ්ඩ ලබාගැනීමේ ප්‍රමාදයන් හෝ ලබා නොගත් උපාංග හේතුවෙන් සිදුවන කිසිදු අලාභයක් සඳහා අප වගකියනු නොලැබේ."));
                    });

                    // --- Column 2: Warranty ---
                    row.RelativeItem().Column(column =>
                    {
                        column.Spacing(5); // Vertical spacing between bullet points

                        // Section Title
                        column.Item().Text("🛠️ වගකීම්").SemiBold().FontSize(8);

                        // List of warranty conditions using the new BulletPoint component
                        column.Item().Element(c => BulletPoint(c, "වගකීම් ආවරණය වන්නේ නිෂ්පාදකයාගේ දෝෂ හා තහවුරු කරන ලද අලුත්වැඩියාවට අදාළ දෝෂ සඳහා පමණි."));
                        column.Item().Element(c => BulletPoint(c, "අනිසි භාවිතය, නොසැලකිලිමත්කම, විදුලි උච්චාවචනයන්, හදිසි අනතුරු, අනවසර වෙනස් කිරීම් හා භෞතික හානි හේතුවෙන් සිදුවන හානි සදහා වගකියනු නොලැබේ."));
                        //column.Item().Element(c => BulletPoint(c, "පිළිස්සුම් සලකුණු, විඛාදනය සහ වගකීම් ස්ටිකර් පළුදු කිරීම වගකීම් අවලංගු කරයි."));
                        column.Item().Element(c => BulletPoint(c, "වගකීම් ආවරණ කාලය, උපාගය හෝ අලුත්වැඩියා වර්ගය අනුව වෙනස් වන අතර එය සේවා වාර්තාවේ සඳහන් කරනු ලැබේ."));
                        column.Item().Element(c => BulletPoint(c, "ඕනෑම වගකීම් හිමිකම් පෑමක් සඳහා මුල් සේවා වාර්තාව ඉදිරිපත් කළ යුතුය."));
                        column.Item().Element(c => BulletPoint(c, "අලුත්වැඩියාව අවසන් දිනයේ සිට සම්පූර්ණ වගකීම් සක්‍රීය කිරීම සහතික කිරීම සඳහා දින 7ක සහන කාලය තුළ උපාංග ලබාගත යුතුය. දින 7ක් ඇතුළත ලබා නොගතහොත්, ගෙවූ දිනය කුමක් වුවත්, අලුත්වැඩියාව අවසන් වී 7 වන දින වගකීම් කාලය ආරම්භ වේ."));
                    });
                });

                // --- A final professional touch ---
                mainColumn.Item().PaddingTop(4).AlignCenter().Text("Thank you for your business!").SemiBold().FontSize(10);


                mainColumn.Item()
                     .ShowEntire() // Ensure it doesn't get cut off between pages
                    .PaddingBottom(0.2f) // Add some space above the signatures

                    .Element(ComposeSignatureArea);

            });
        }




        // A new helper to safely load the PAID stamp image to keep the main method clean.
        private byte[] GetPaidStampBytes()
        {
            try
            {
                 string stampimagePath = ConfigurationManager.AppSettings["paidstamppath"] ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", "paid-stamp.png");

                if (File.Exists(imagePath))
                {
                    return File.ReadAllBytes(stampimagePath);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Could not load PAID stamp image: {ex.Message}");
            }
            // Return an empty byte array if the image fails to load, preventing a crash.
            return new byte[0];
        }


        //a new helper to safely load the signature image to keep the main method clean.
        private byte[] GetSignatureBytes()
        {
            try
            {
                string signatureImagePath = ConfigurationManager.AppSettings["signaturepath"] ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", "signature.png");
                if (File.Exists(signatureImagePath))
                {
                    return File.ReadAllBytes(signatureImagePath);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Could not load signature image: {ex.Message}");
            }
            // Return an empty byte array if the image fails to load, preventing a crash.
            return new byte[0];
        }




        // NEW HELPER METHOD FOR THE FINAL SIGNATURE BLOCK
        private void ComposeSignatureArea(IContainer container)
        {
            //container.Row(signatureRow =>
            //{
            //    Action<IContainer, string> signatureBlock = (blockContainer, label) =>
            //    {
            //        blockContainer.AlignCenter().PaddingVertical(1).Column(col =>
            //        {
            //            col.Spacing(5);
            //            col.Item().Text("_________________________").FontSize(8);
            //            col.Item().Text(label).SemiBold().FontSize(8);
            //        });
            //    };
            //    //add a paid background watermark if the job is paid on the top of authrized by signature
            //    //add the stamp only as a background image to the authorized by signature block container otherwise it will overlap the customer signature block



            //    signatureRow.RelativeItem().PaddingVertical(1).Element(c => signatureBlock(c, "Authorized by"));
            //    signatureRow.RelativeItem().PaddingVertical(1).Element(c => signatureBlock(c, "Customer Signature"));
            //});


            container.Row(signatureRow =>
            {
                Action<IContainer, string> signatureBlock = (blockContainer, label) =>
                {
                    blockContainer.AlignCenter().PaddingVertical(1).Column(col =>
                    {
                        col.Spacing(5);
                        col.Item().Text("_________________________").FontSize(8);
                        col.Item().Text(label).SemiBold().FontSize(8);
                    });
                };

                // Left side: "Authorized by" - check if we need layers or simple element
                if (_includeStampsAndSignatures && _loadedJobWithDetails?.Billing?.IsPaid == true)
                {
                    // Use Layers only when we need to overlay signature
                    signatureRow.RelativeItem().PaddingVertical(1).Layers(layers =>
                    {
                        // Background layer: The normal signature block
                        layers.Layer().Element(c => signatureBlock(c, "Authorized by"));

                        // Primary layer: Signature image (required for Layers to work)
                        try
                        {
                            byte[] signatureBytes = GetSignatureBytes();
                            if (signatureBytes.Length > 0)
                            {
                                layers.PrimaryLayer()
                                    .AlignCenter()
                                    .AlignTop()
                                    .PaddingTop(-10)
                                    .Height(30)
                                    .Width(80)
                                    .Image(signatureBytes);
                            }
                            else
                            {
                                // If signature image fails to load, use empty primary layer
                                layers.PrimaryLayer().Height(0);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Could not load signature image: {ex.Message}");
                            // Add empty primary layer to satisfy QuestPDF requirement
                            layers.PrimaryLayer().Height(0);
                        }
                    });
                }
                else
                {
                    // For print version or unpaid jobs, use simple element without layers
                    signatureRow.RelativeItem().PaddingVertical(1).Element(c => signatureBlock(c, "Authorized by"));
                }

                // Right side: Customer signature (unchanged)
                signatureRow.RelativeItem().PaddingVertical(1).Element(c => signatureBlock(c, "Customer Signature"));
            });


        }
















        //pdf generation using QuestPDF library
        private byte[] GenerateInvoicePdf() // Returns byte array of the PDF
        {
            QuestPDF.Settings.License = LicenseType.Community; // Or your license

            // I've removed the ComposeHeader part for this example to be self-contained.
            // If you have it, it will work fine with the rest of the code.

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    
                    page.Size(PageSizes.A4);
                    page.Margin(2.5f, Unit.Millimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Helvetica"));


                   


                    // This background layer is drawn on EVERY page, behind the content.
                    page.Background()
                        .AlignCenter()  // Horizontally center the watermark
                        .AlignMiddle()  // Vertically center the watermark
                        .Column(col => // Using a column to potentially add text later if needed
                        {
                            //also add a paid stamp background image only once below the composelineitems table if the job is paid
                            if (_includeStampsAndSignatures && _loadedJobWithDetails?.Billing?.IsPaid == true)
                            {
                                try
                                {
                                    byte[] paidStampBytes = GetPaidStampBytes();
                                    if (paidStampBytes.Length > 0)
                                    {
                                        col.Item()
                                            .ShowOnce() // Only show on the first page
                                            .AlignRight()
                                            .AlignTop()
                                            .PaddingTop(-225) // Position below header
                                            .PaddingRight(-50)
                                            .Rotate(-10)
                                            .Width(170)
                                            .Image(paidStampBytes);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine($"Could not load PAID stamp: {ex.Message}");
                                }
                            }



                            try
                            {
                                // We load the logo image again for the watermark.

                                //background watermark image removed on 2026.03.25  because of a client request but the code is kept here for future use if needed just commenting out the watermark image part in the background layer and it will work without any issues
                                //string imagePath = ConfigurationManager.AppSettings["watermarkpath"] ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", "logo.png");
                                //byte[] watermarkBytes = File.ReadAllBytes(imagePath);

                               // col.Item().Width(300).AlignCenter() // Set a fixed width for the watermark
                                //   .Image(watermarkBytes);


                                   //add transparency to the watermark image

                                   //.Opacity(0.02f); // Set the opacity to 2% (very faint)
                            }
                            catch (Exception ex)
                            {
                                // If the logo file is missing, this will prevent a crash.
                                // You can log the error for debugging.
                                Debug.WriteLine($"Watermark image not found: {ex.Message}");
                            }
                        });




                    page.Header().ShowOnce().Element(ComposeHeader); // Assuming you have this method defined elsewhere
                    // For a runnable example, let's create a minimal header here:
                    //page.Header().Row(row => {
                    //    row.RelativeItem().Text("Edison Computers").Bold().FontSize(18).FontColor(Colors.Red.Medium);
                    //});
                    //});


                    page.Content()
                        .PaddingVertical(0.2f, Unit.Centimetre)
                        .Column(column =>
                        {

                            column.Spacing(8); // Vertical spacing between major sections

                            //column.Item().Unbreakable().Element(ComposeTermsAndWarranty);

                            //add the helper methods to compose the invoice content


                            // 1. Add the new two-column section
                            column.Item().Element(ComposeCustomerAndInvoiceDetails);

                            // 2. Add the device details section
                            //column.Item().Element(ComposeDeviceDetails);

                            // 3. Add the main table of services and parts
                            column.Item().Element(ComposeLineItemsTable);

                           

                            //column.Item().PageBreak();
                            //column.Item().Element(ComposeTermsAndWarranty);
                            // ---- NEW INTEGRATION POINT ----
                            // 5. Add the Terms & Warranty section at the very bottom
                            column.Item().ShowEntire().Element(ComposeTermsAndWarranty);



                            // --- THE FINAL AND CORRECT WAY TO ADD SIGNATURES ---
                            // Add the signature block as the final piece of content.
                            // It will appear after the terms and conditions, on the same page if it fits,
                            // or on the next page if it doesn't.
                         



                        });

                    // ... (page footer remains the same) ...
                    // SIMPLIFIED, STABLE FOOTER
                    page.Footer().Column(footerColumn =>
                    {
                        footerColumn.Spacing(5);

                        // The decorative line that appears on EVERY page.
                        footerColumn.Item().PaddingTop(1).LineHorizontal(5f).LineColor(Colors.Red.Accent3);

                        // The page number that appears on EVERY page.
                        footerColumn.Item().AlignCenter().Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" of ");
                            x.TotalPages();
                        });
                    });
                });
            }).GeneratePdf();
        }


        //-----------------end of invoice header and footer generation-----------------//

        //this download event removed because of the download pdf button is in the webui2 control

        //private void btndownloadPDF_Click(object sender, EventArgs e)
        //{
        //    //call the method to generate the PDF
        //    byte[] pdfBytes = GenerateInvoicePdf();
        //    if (pdfBytes == null || pdfBytes.Length == 0)
        //    {
        //        MessageBox.Show("Failed to generate PDF.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }
        //    else
        //    {
        //        // Save the PDF to a file
        //        using (var saveFileDialog = new SaveFileDialog())
        //        {
        //            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
        //            saveFileDialog.FileName = $"Invoice_{_jobId}.pdf"; // Default filename
        //            if (saveFileDialog.ShowDialog() == DialogResult.OK)
        //            {
        //                try
        //                {
        //                    File.WriteAllBytes(saveFileDialog.FileName, pdfBytes);
        //                    MessageBox.Show("Invoice PDF saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show($"Error saving PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                }
        //            }
        //        }
        //    }


        //}

        //a method to send generated PDF via email
        private async void btnsendEmail_Click(object sender, EventArgs e)
        {

            //this event hanlder was removed because of the email sending button is in the webui2 control
        }



        //a method to send generated PDF via Whatsapp 
        private void btnsendviaWhatsapp_Click(object sender, EventArgs e)
        {

            //this event hanlder was removed because of the whatsapp sending button is in the webui2 control

        }







        //if the user dont have email or whatsapp then send the invoice via SMS
        private void btnsendSMS_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(_customerDetails?.Phone)) { /* ... */ return; }

            // Build the SMS message content
            string smsBody = $"Edison Computers Invoice for Job #{_jobId}:\n" +
                             $"Device: {_loadedJobWithDetails.Device.Model}\n" +
                             $"Total Amount: {_loadedJobWithDetails.Billing.Amount:C2} LKR\n"; // Assuming LKR
            if (_loadedJobWithDetails.Billing.IsPaid == true)
            {
                smsBody += "Status: PAID";
            }
            else
            {
                smsBody += "Status: UNPAID. Please arrange payment.";
            }
            smsBody += "\nThank you!";

            // --- Placeholder for actual SMS sending logic ---
            // This is where you would integrate with an SMS Gateway API
            try
            {
                // Example using a hypothetical SmsService
                // using (var scope = _serviceProvider.CreateScope())
                // {
                //    var smsService = scope.ServiceProvider.GetRequiredService<ISmsSenderService>();
                //    bool success = await smsService.SendSmsAsync(_customerDetails.Phone, smsBody);
                //    if (success) MessageBox.Show("Invoice summary sent via SMS!", "SMS Sent");
                //    else MessageBox.Show("Failed to send SMS.", "SMS Error");
                // }
                MessageBox.Show($"SMS to send to {_customerDetails.Phone}:\n\n{smsBody}\n\n(SMS Gateway integration needed)", "SMS Preview");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error preparing SMS: {ex.Message}", "SMS Error");
            }

        }


        //print method 
        private async void btnPrint_Click(object sender, EventArgs e)
        {
            if (webViewPriview != null && webViewPriview.CoreWebView2 != null)
            {
                // This command opens the standard browser print dialog for the PDF
                await webViewPriview.CoreWebView2.ExecuteScriptAsync("window.print();");
            }
        }


        //new pdf download button click event handler for the web view control
        private void btndownloadPdfDoc_Click(object sender, EventArgs e)
        {
            if (_pdfBytes == null || _pdfBytes.Length == 0)
            {
                MessageBox.Show("PDF data is not available to download.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                saveFileDialog.FileName = $"Invoice_{_jobId}.pdf";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllBytes(saveFileDialog.FileName, _pdfBytes);
                        MessageBox.Show("Invoice PDF saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        // --- NEW: Form Closing event to clean up the temporary file ---
        private void InvoiceDialog_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (!string.IsNullOrEmpty(_tempPdfPath) && File.Exists(_tempPdfPath))
            {
                try
                {
                    // It's good practice to dispose the WebView2 control to release file locks before deleting
                    webViewPriview?.Dispose();
                    File.Delete(_tempPdfPath);
                }
                catch (IOException ex)
                {
                    // Log or ignore if the file can't be deleted, as it's in the temp folder
                    // and will eventually be cleaned up by the OS.
                    Debug.WriteLine($"Could not delete temp PDF file: {ex.Message}");
                }
            }
        }






        //new button click event handler for sending invoice via WhatsApp WebView
        private void btnsendviaWhatsappWebView_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_customerDetails?.Phone))
            {
                MessageBox.Show("Customer phone number is missing.", "WhatsApp Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            byte[] pdfBytes = GenerateInvoicePdf();
            //string tempPdfPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"Invoice_{_jobId}.pdf"); // Save to Documents
            string tempPdfPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), string.Format(invoicePdfDocPattern, _jobId));
            try
            {
                File.WriteAllBytes(tempPdfPath, pdfBytes);
                // Clean up phone number for wa.me link (remove +, spaces, hyphens if present)
                string phoneNumber = new string(_customerDetails.Phone.Where(char.IsDigit).ToArray());
                if (!phoneNumber.StartsWith("94") )// e.g., "94" for Sri Lanka
                {
                    // Attempt to prepend if it's a local number format
                    // This logic needs to be country-specific
                    phoneNumber = "94" + phoneNumber; // Assuming local numbers start with 0 or similar
                }

                string message = Uri.EscapeDataString($"Dear {_customerDetails.FirstName}, your invoice for Job #{_jobId} from Edison Computers is ready. Please find the PDF attached. If you have any questions, feel free to contact us. Thank you!");
                //Process.Start(new ProcessStartInfo($"https://wa.me/{phoneNumber}?text={message}") { UseShellExecute = true });
                Process.Start(new ProcessStartInfo($"{whatsAppBaseUrl}{phoneNumber}?text={message}") { UseShellExecute = true });

                MessageBox.Show($"Invoice PDF saved to: {tempPdfPath}\nWhatsApp will open. Please attach the PDF manually.", "WhatsApp Prepared");
            }
            catch (Exception ex) { MessageBox.Show($"Error preparing for WhatsApp: {ex.Message}"); }
        }






        //new button click event handler for sending invoice via email WebView

        private async void btnsendEmailWebView_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(_customerDetails?.Email))
            {
                MessageBox.Show("Customer email is not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string tempPdfPath = string.Empty; // Declare here to access in finally

            try
            {
                byte[] pdfBytes = GenerateInvoicePdf(); // Assuming this returns the PDF bytes correctly
                tempPdfPath = Path.Combine(Path.GetTempPath(), $"Invoice_{_jobId}_{DateTime.Now:yyyyMMddHHmmss}.pdf");
                File.WriteAllBytes(tempPdfPath, pdfBytes); // Write the PDF to a temporary file

                // Use 'using' statements for IDisposable objects
                using (MailMessage mail = new MailMessage())
                using (SmtpClient smtpServer = new SmtpClient("smtp.gmail.com")) // Your SMTP server
                {
                    mail.From = new MailAddress(shopEmail, "Edison Computers"); // Ensure shopEmail is defined
                    mail.To.Add(_customerDetails.Email);
                    mail.Subject = $"Invoice {_jobId} from Edison Computers";
                    mail.Body = $"Dear {_customerDetails.FirstName},\n\nPlease find attached your invoice for job #{_jobId}.\n\nThank you,\nEdison Computers";

                    // Create attachment within its own using block if it holds the stream strongly
                    // Or ensure mail.Dispose() handles its attachments.
                    // For safety, it's good to manage the attachment stream explicitly if issues persist.
                    // However, MailMessage.Dispose() should dispose its attachments.
                    using (Attachment attachment = new Attachment(tempPdfPath))
                    {
                        mail.Attachments.Add(attachment);

                        smtpServer.Port = smtpPort; // Standard port for TLS
                        // smtpServer.Port = 587; // Standard port for TLS
                        smtpServer.Credentials = new System.Net.NetworkCredential(shopEmail, shopPassword); // Ensure shopEmail and shopPassword are defined
                        smtpServer.EnableSsl = true; // Use SSL/TLS

                        await smtpServer.SendMailAsync(mail); // Asynchronously send the email
                    } // Attachment is disposed here, releasing the file lock if it held one.

                    MessageBox.Show("Invoice sent via Email successfully!", "Email Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } // MailMessage and SmtpClient are disposed here
            }
            catch (SmtpException smtpEx) // Catch more specific exceptions
            {
                // Log the full smtpEx for detailed error info (e.g., status code)
                Console.WriteLine($"SMTP Error: {smtpEx.ToString()}");
                MessageBox.Show($"Failed to send email due to an SMTP error: {smtpEx.Message}", "Email Send Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ioEx) // Catch file access errors specifically
            {
                Console.WriteLine($"IO Error: {ioEx.ToString()}");
                MessageBox.Show($"Failed to send email due to a file access error: {ioEx.Message}", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) // Catch any other general exceptions
            {
                // Log the full ex for detailed error info
                Console.WriteLine($"General Error: {ex.ToString()}");
                MessageBox.Show($"Failed to send email: {ex.Message}", "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Clean up the temporary PDF file
                if (!string.IsNullOrEmpty(tempPdfPath) && File.Exists(tempPdfPath))
                {
                    try
                    {
                        File.Delete(tempPdfPath);
                    }
                    catch (IOException ex)
                    {
                        // This might happen if the lock is *still* somehow held, though less likely with proper using statements.
                        // Or if antivirus is scanning it.
                        Console.WriteLine($"Could not delete temp file {tempPdfPath}: {ex.Message}");
                        // Optionally inform the user that the temp file couldn't be deleted automatically.
                    }
                }
            }


        }
    }

    // Helper extension method for truncating strings (optional)
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }
    }



    // NEW DYNAMIC COMPONENT CLASS FOR THE SIGNATURES (Corrected for your QuestPDF version)
    //public class LastPageFooterSignature : IDynamicComponent
    //{
    //    // CORRECTED: The method must return DynamicComponentComposeResult
    //    public DynamicComponentComposeResult Compose(DynamicContext context)
    //    {
    //        // THE CRITICAL LOGIC: Only render if we are on the last page.
    //        if (context.PageNumber == context.TotalPages)
    //        {
    //            // First, create the content element just like before.
    //            var content = context.CreateElement(container =>
    //            {
    //                container
    //                    .PaddingTop(10) // Add space above the signature lines
    //                    .Row(signatureRow =>
    //                    {
    //                        Action<IContainer, string> signatureBlock = (blockContainer, label) =>
    //                        {
    //                            blockContainer.AlignCenter().Column(col =>
    //                            {
    //                                col.Spacing(5);
    //                                col.Item().Text("_________________________").FontSize(10);
    //                                col.Item().Text(label).SemiBold().FontSize(8);
    //                            });
    //                        };

    //                        // Left Side: Authorized by
    //                        signatureRow.RelativeItem().Element(c => signatureBlock(c, "Authorized by"));

    //                        // Right Side: Customer Signature
    //                        signatureRow.RelativeItem().Element(c => signatureBlock(c, "Customer Signature"));
    //                    });
    //            });

    //            // NOW, RETURN THE RESULT OBJECT containing the content.
    //            // This fulfills the interface requirement.
    //            return new DynamicComponentComposeResult
    //            {
    //                Content = content,
    //                HasMoreContent = false // The signature block never spans multiple pages.
    //            };
    //        }

    //        // If it's NOT the last page, return an empty result.
    //        // This tells QuestPDF to draw nothing for this component on this page.
    //        return new DynamicComponentComposeResult
    //        {
    //            Content = null, // No content to render
    //            HasMoreContent = false // No more content to render
    //        };

    //    }
    //}





















}
