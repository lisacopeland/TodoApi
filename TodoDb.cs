using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace TodoApi
{
    public class Foo
    {
        public async void Bar()
        {
            Horse trigger = new Horse("LoneRanger", "Trigger", "Palimino", "Quarter");

            Horse success = await DBCall.WriteAsync<Horse>("horses", trigger);

            List<Horse> horses = await DBCall.Query<Horse>("horses", "LoneRanger", "Trigger");
        }
    }

    public static class DBCall
    {
        static readonly AmazonDynamoDBClient ddbClient;
        static DBCall()
        {
            ddbClient = new AmazonDynamoDBClient();
        }

        public static async Task<T> WriteAsync<T>(string tableName, T item) where T : DatabaseItem
        {
            var itemData = item.GetItemData();

            PutItemRequest request = new PutItemRequest()
            {
                TableName = tableName,
                Item = item.GetItemData()
            };

            PutItemResponse response = await ddbClient.PutItemAsync(request);


            return item;
        }

        public static async Task<bool> DeleteAsync(string tableName, string Pk, string Sk)
        {
            Dictionary<string, AttributeValue> expressionValues = new Dictionary<string, AttributeValue>()
            {
                { "Pk", new AttributeValue { S = Pk } },
                { "Sk", new AttributeValue { S = Sk } }
            };

            // Create DeleteItem request
            DeleteItemRequest request = new DeleteItemRequest
            {
                TableName = tableName,
                Key = expressionValues
            };

            // Issue request
            DeleteItemResponse result = await ddbClient.DeleteItemAsync(request);
            return true;
        }


        public static async Task<List<T>> Query<T>(string tableName, string pk, string? sk = null) where T : DatabaseItem
        {
            string keyCondition = "Pk = :pk";
            var expressionValues = new Dictionary<string, AttributeValue>()
            {
                { ":pk", new AttributeValue() { S = pk } }
            };

            if (sk != null)
            {
                keyCondition += " and Sk = :sk";
                expressionValues.Add(":sk", new AttributeValue() { S = sk });
            }
            QueryRequest request = new QueryRequest()
            {
                TableName = tableName,
                KeyConditionExpression = keyCondition,
                ExpressionAttributeValues = expressionValues
            };

            QueryResponse response = await ddbClient.QueryAsync(request);

            List<T> toReturn = new List<T>();
            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                DatabaseItem newItem = DatabaseItem.CreateItem(typeof(T).Name);
                newItem.Populate(item);
                toReturn.Add((T)newItem);
            }
            return toReturn;
        }

        public static async Task<List<T>> QueryByIndex<T>(string tableName, string pk, string index) where T : DatabaseItem
        {
            string keyCondition = "Pk = :pk";
            var expressionValues = new Dictionary<string, AttributeValue>()
            {
                { ":pk", new AttributeValue() { S = pk } }
            };

            QueryRequest request = new QueryRequest()
            {
                TableName = tableName,
                IndexName = index,
                KeyConditionExpression = keyCondition,
                ExpressionAttributeValues = expressionValues
            };

            QueryResponse response = await ddbClient.QueryAsync(request);

            List<T> toReturn = new List<T>();
            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                DatabaseItem newItem = DatabaseItem.CreateItem(typeof(T).Name);
                newItem.Populate(item);
                toReturn.Add((T)newItem);
            }
            return toReturn;
        }

    }


    public abstract class DatabaseItem
    {
        public abstract Dictionary<string, AttributeValue> GetItemData();
        public abstract bool Populate(Dictionary<string, AttributeValue> data);

        public static DatabaseItem CreateItem(string name)
        {
            if (name.Equals("Horse")) return new Horse();

            if (name.Equals("TodoItem")) return new TodoItem();

            throw new InvalidOperationException("Forgot to add new type to CreateItem method!!!");
        }

    }

    public class Horse : DatabaseItem
    {
        public string Owner { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }

        public Horse() { }
        public Horse(string owner, string name, string color, string breed)
        {
            this.Owner = owner;
            this.Name = name;
            this.Breed = breed;
            this.Color = color;
        }


        public override Dictionary<string, AttributeValue> GetItemData()
        {
            var data = new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue() { S = this.Owner } },
                { "SK", new AttributeValue() { S = this.Name } },
                { "Owner", new AttributeValue() { S = this.Owner } },
                { "Name", new AttributeValue() { S = this.Name } },
                { "Breed", new AttributeValue() { S = this.Breed } },
                { "Color", new AttributeValue() { S = this.Color } }
            };

            return data;
        }

        public override bool Populate(Dictionary<string, AttributeValue> data)
        {
            foreach (KeyValuePair<string, AttributeValue> pair in data)
            {
                // Look at key and fill in the data
                if (pair.Key.Equals("Name"))
                {
                    this.Name = pair.Value.S;
                }
            }
            return true;
        }
    }
}
