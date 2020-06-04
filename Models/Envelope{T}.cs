using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ToDoListRecruitment.Models
{
    public class Envelope<T>
    {
        public bool success { get; set; }
        public int status { get; set; }
        public object message { get; set; }
        public string url { get; set; }
        public T data { get; set; }
        public int totalData { get; set; }
    }

    public class TempRes
    {
        public static IActionResult result(int code, string uri, object data = null, object msg = null, int totalData = 0){
            
            var env = new Envelope<object> {
                success = false,
                status = code,
                message = msg == null ? "" : msg,
                url = uri,
                data = data == null ? new object() : data,
                totalData = totalData
            };

            var okObject = new OkObjectResult(env);

            switch(code) {
                case 200:
                    env.message = msg != null ? msg : "Success";
                    env.success = true;
                    return new OkObjectResult(env);
                case 404:
                    env.message = msg != null ? msg : "Data not found";
                    return new NotFoundObjectResult(env);
                case 400:
                    env.message = msg != null ? msg : "Invalid Request";
                    return new BadRequestObjectResult(env);
                case 401:
                    env.message = msg != null ? msg : "Unauthorized";
                    okObject.StatusCode = StatusCodes.Status401Unauthorized;
                    break;
            }

            env.message = msg != null ? msg : code + " Error";
            okObject.StatusCode = code;
            return okObject;
        }

        public static string getErrString(int code){
            switch(code) {
                case 404:
                    return "Data not found";
                default :
                    return code + " Error";
            }
        }

        public static Envelope<object> notfound(string uri, object msg = null){
            if(msg == null) {
                msg = getErrString(404);
            }
            return new Envelope<object> {
                success = false,
                status = 404,
                message = msg,
                url = uri,
                data = new object()
            };
        }

        public static Envelope<object> badreq(string uri, string msg = "Invalid Request"){
            return new Envelope<object> {
                success = false,
                status = 400,
                message = msg,
                url = uri,
                data = new object()
            };
        }

        public static Envelope<object> unauth(string uri, string msg = "Unauthorized"){
            return new Envelope<object> {
                success = false,
                status = 401,
                message = msg,
                url = uri,
                data = new object()
            };
        }

        public static Envelope<object> forbiden(string uri, string msg = "Forbiden"){
            return new Envelope<object> {
                success = false,
                status = 403,
                message = msg,
                url = uri,
                data = new object()
            };
        }

        public static Envelope<object> success(object datas, string uri, string msg = "Success"){
            return new Envelope<object> {
                success = true,
                status = 200,
                message = msg,
                url = uri,
                data = datas
            };
        }
    }
}