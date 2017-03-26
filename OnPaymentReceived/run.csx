#r "Newtonsoft.Json"

using System;
using System.Net;
using Newtonsoft.Json;

public class Order
{
    public string PartitionKey {get; set;}
    public string RowKey {get; set;}
    public string OrderId {get; set;}
    public string ProductId {get; set;}
    public string Email {get; set;}
    public decimal Price {get; set;}
}

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log, IAsyncCollector<Order> outputQueueItem, IAsyncCollector<Order> outputTable)
{
    /*
    log.Info($"Webhook was triggered!");

    string jsonContent = await req.Content.ReadAsStringAsync();
    dynamic data = JsonConvert.DeserializeObject(jsonContent);

    if (data.first == null || data.last == null) {
        return req.CreateResponse(HttpStatusCode.BadRequest, new {
            error = "Please pass first/last properties in the input object"
        });
    }

    return req.CreateResponse(HttpStatusCode.OK, new {
        greeting = $"Hello {data.first} {data.last}!"
    });
    */

    log.Info($"Order received");
    string jsonContent = await req.Content.ReadAsStringAsync();
    var order = JsonConvert.DeserializeObject<Order>(jsonContent);
    log.Info($"Order {order.OrderId} received from {order.Email} for product {order.ProductId}");

    order.PartitionKey = "Orders";
    order.RowKey = order.OrderId;


    await outputQueueItem.AddAsync(order);
    await outputTable.AddAsync(order);

    return req.CreateResponse(HttpStatusCode.OK, new {message = $"Thank you greatly for your order of product {order.ProductId}!"});

}
