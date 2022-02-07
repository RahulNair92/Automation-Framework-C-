using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace UdemyRestSharpNetCore.Utilities
{
    public static class Libraries
    {

        public static Dictionary<string, string> DeserializeResponse(this RestResponse restResponse)
           
        {
            var JSONObj =  JsonConvert.DeserializeObject<Dictionary<string, string>>(restResponse.Content);

            return JSONObj;
        }

        public static string GetResponseObject(this RestResponse response, string responseObject)
        {
            JObject obs = JObject.Parse(response.Content);
            return obs[responseObject].ToString();
        }

        public static string GetResponseObjectv2(this RestResponse response, string responseObject)
        {
            JObject obs = JObject.Parse(response.Content.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' }));
            return obs[responseObject].ToString();
        }



        public static string GetResponseObjectArray(this RestResponse response, string responseObject)
        {


            JArray jArray = JArray.Parse(response.Content);
            foreach (var content in jArray.Children<JObject>())
            {
                foreach (JProperty property in content.Properties())
                {
                    if (property.Name == responseObject)
                        return property.Value.ToString();
                }
            }

            return string.Empty;
        }

        //public static async Task<IRestResponse<T>> ExecuteAsyncRequest<T>(this RestClient client, IRestRequest request) where T : class, new()
        //{
        //    var taskCompletionSource = new TaskCompletionSource<IRestResponse<T>>();

        //    client.ExecuteAsync<T>(request, restResponse =>
        //    {
        //        if (restResponse.ErrorException != null)
        //        {
        //            const string message = "Error retrieving response.";
        //            throw new ApplicationException(message, restResponse.ErrorException);
        //        }

        //        taskCompletionSource.SetResult(restResponse);
        //    });

        //    return await taskCompletionSource.Task;
        //}



        public static async Task<RestResponse> ExecuteAsyncRequests<T>(this RestClient client, RestRequest request) where T : class, new()
        {
            return await client.ExecuteAsync(request);
        }

    }
}
