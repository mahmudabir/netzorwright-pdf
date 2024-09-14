namespace NetzorwrightPdf.TestConsole;

public class InvoiceViewModel
{
    public string ShopName { get; set; }
    public string ShopAddress { get; set; }
    public string ShopEmail { get; set; }
    public string ShopPhone { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string InvoiceNumber { get; set; }
    public List<InvoiceItem>? Items { get; set; } = new List<InvoiceItem>();
    public decimal Subtotal => Items?.Sum(x => x.UnitPrice) ?? 0;
    public decimal TaxRate { get; set; }
    public decimal TaxAmount => Subtotal * (TaxRate / 100);
    public decimal Total => Subtotal + TaxAmount;
}

public class InvoiceItem
{
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

