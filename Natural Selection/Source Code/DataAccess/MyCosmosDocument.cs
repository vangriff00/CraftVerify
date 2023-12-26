using System;

public class MyCosmosDocument
{
    // Required property: The unique identifier for the document.
    public string Id { get; set; }

    // Your custom properties specific to your document.
    public string PartitionKey { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    // Add any other properties as needed.

    // You can include any methods or additional logic here if necessary.
}

