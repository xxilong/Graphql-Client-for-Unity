using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Command
{
    public InterpretCommand interpretCommand { get; set; }  
}

public class InterpretCommand
{
    public bool parsed { get; set; }
    public string json { get; set; }
    public string jsonDesp { get; set; }
}

public class Revision
{    public float revision { get; set; }
}

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        string uri = "http://127.0.0.1:24000/";
        using var graphQLClient = new GraphQLHttpClient(new Uri(uri), new NewtonsoftJsonSerializer());

        var queryRequest = new GraphQLRequest
        {
            Query = @"
              query Query {
              revision
            }",
            //OperationName = "mutation",
            //Variables = new
            //{
            //    buf = "qJIlAWUBAQABSoEGQZUxKTGHSgAQABUJAw8FIAIIAQABAQAAAAAAAKk="
            //}
        };
        var mutationRequest = new GraphQLRequest
        {
            Query = @"
            mutation InterpretCommand($buf: String!) {
              interpretCommand(buf: $buf) {
                parsed
                json
                jsonDesp
              }
            }",
            //OperationName = "Test",
            //Variables = JObject.Parse("{\"buf\":\"qJIlAWUBAQABSoEGQZUxKTGHSgAQABUJAw8FIAIIAQABAQAAAAAAAKk=\"}");
            Variables =new
            {
                buf = "qJIlAWUBAQABSoEGQZUxKTGHSgAQABUJAw8FIAIIAQABAQAAAAAAAKk="
            }
        };

        Debug.Log($"Variables:{JsonConvert.SerializeObject(mutationRequest.Variables)}");

        var graphQLResponse = await graphQLClient.SendMutationAsync<Command>(mutationRequest);
        //var graphQLResponse = await graphQLClient.SendQueryAsync<Revision>(queryRequest);
        if (graphQLResponse.Errors!=null)
        {
            Debug.LogError(JsonConvert.SerializeObject(graphQLResponse.Errors));
        }        

        Debug.Log("raw response:");
        Debug.Log($"{JsonConvert.SerializeObject(graphQLResponse)}");
        string json = JsonConvert.SerializeObject(graphQLResponse.Data);
        Debug.Log($"Data: {json}");
    }
}
