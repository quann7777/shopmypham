using System.Web.Mvc;

namespace WebBanHangOnline.Controllers
{
    public class ChatBotController : Controller
    {
        [HttpPost]
        public JsonResult GetReply(MessageModel model)
        {
            // TODO: Thay phần này bằng gọi DB / OpenAI API
            string userMsg = model.Message;
            string reply = $"Bạn vừa hỏi: \"{userMsg}\"";
            return Json(new { reply });
        }

        public class MessageModel
        {
            public string Message { get; set; }
        }
    }
}
