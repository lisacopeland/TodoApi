using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace TodoApi;

public class TodoItem : DatabaseItem
{
    public string? Id { get; set; }
    public DateTime? DateAdded { get; set; }
    public DateTime? DueDate { get; set; }

    public string? Username { get; set; }
    public string? Task { get; set; }

    public string? Priority { get; set; }

    public TodoItem(DateTime dueDate, string userName, string task, string priority = "5")
    {
        this.Id = Guid.NewGuid().ToString();
        this.DateAdded = DateTime.Now;
        this.DueDate = dueDate;
        this.Username = userName;
        this.Task = task;
        this.Priority = priority;
    }

    public TodoItem(TodoItem todoItem)
    {
        this.Id = todoItem.Id;
        this.DateAdded = todoItem.DateAdded;
        this.DueDate = todoItem.DueDate;
        this.Username = todoItem.Username;
        this.Task = todoItem.Task;
        this.Priority = todoItem.Priority;
    }

    public TodoItem() { }
    public override Dictionary<string, AttributeValue> GetItemData()
    {
        var data = new Dictionary<string, AttributeValue>
            {
                { "Pk", new AttributeValue() { S = this.Username } },
                { "Sk", new AttributeValue() { S = this.Id } },
                { "Id", new AttributeValue() { S = this.Id } },
                { "Username", new AttributeValue() { S = this.Username } },
                { "DateAdded", new AttributeValue() { S = this.DateAdded.ToString() } },
                { "DueDate", new AttributeValue() { S = this.DueDate.ToString() } },
                { "Task", new AttributeValue() { S = this.Task } },
                { "Priority", new AttributeValue() { S = this.Priority.ToString() }}
            };

        return data;
    }

    public override bool Populate(Dictionary<string, AttributeValue> data)
    {
        foreach (KeyValuePair<string, AttributeValue> pair in data)
        {
            // Look at key and fill in the data
            var key = pair.Key;
            switch (key)
            {

                case "Id": 
                    this.Id = pair.Value.S;
                    break;
                case "Username": // statement sequence
                    this.Username = pair.Value.S;
                    break;
                case "DateAdded": // statement sequence
                    this.DateAdded = DateTime.Parse(pair.Value.S);
                    break;
                case "DueDate": // statement sequence
                    this.DueDate = DateTime.Parse(pair.Value.S);
                    break;                    
                case "Task": // statement sequence
                    this.Task = pair.Value.S;
                    break;
                case "Priority": // statement sequence
                    this.Priority = pair.Value.S;
                    break;
                default:    // default statement sequence
                    break;
            }
        }
        return true;
    }

}
