using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ToDoListRecruitment.Enums;
using ToDoListRecruitment.Models;

namespace ToDoListRecruitment.Controllers.API
{
    [Route("api/[controller]")]
    public class ListController : Controller
    {
        private readonly ApiDb _db;
        private List.Query _listQ;

        public ListController(ApiDb db)
        {
            _db = db;
            _listQ = new List.Query(db);
        }

        [HttpPost]
        public async Task<IActionResult> post([FromBody] ListInsert list)
        {
            if (list == null) return TempRes.result(400, Request.GetDisplayUrl(), list, "Data List Tidak Boleh Kosong");

            // MODELSTATE BASED VALIDATION
            ModelState.Clear();
            TryValidateModel(list);
            if (!ModelState.IsValid)
            {
                return TempRes.result(400, Request.GetDisplayUrl(), list, ModelState.Values.SelectMany(ms => ms.Errors).FirstOrDefault().ErrorMessage);
            }

            var newlist = await _listQ.insert(list);

            return TempRes.result(200, Request.GetDisplayUrl(), newlist);
        }

        [HttpPut]
        public async Task<IActionResult> put([FromBody] ListUpdate list)
        {
            if (list == null) return TempRes.result(400, Request.GetDisplayUrl(), list, "Data List Tidak Boleh Kosong");

            // MODELSTATE BASED VALIDATION
            ModelState.Clear();
            TryValidateModel(list);
            if (!ModelState.IsValid)
            {
                return TempRes.result(400, Request.GetDisplayUrl(), list, ModelState.Values.SelectMany(ms => ms.Errors).FirstOrDefault().ErrorMessage);
            }

            var listupdate = await _listQ.update(list);
            if (listupdate == null) return TempRes.result(404, Request.GetDisplayUrl(), list, "Data List Tidak Ditemukan");

            return TempRes.result(200, Request.GetDisplayUrl(), listupdate);
        }

        [HttpGet]
        public async Task<IActionResult> get([FromQuery] string key, [FromQuery] string order, [FromQuery] bool? ascending = false, [FromQuery] int? page = 1, [FromQuery] int? size = 10, [FromQuery] statusList statusList = statusList.Active)
        {
            var lists = await _listQ.getAll(key: key, order: order, ascending: (bool)ascending, page: (int)page, size: (int)size, statusList: statusList);
            var listcount = await _listQ.getAllCount(key: key, order: order, ascending: (bool)ascending, statusList: statusList);

            return TempRes.result(200, Request.GetDisplayUrl(), lists, null, listcount);
        }

        [HttpGet("{idList}")]
        public async Task<IActionResult> getDetail(long idList)
        {
            var list = await _listQ.getDetail(idList);
            if(list == null) return TempRes.result(404, Request.GetDisplayUrl(), list, "Data List Tidak Ditemukan");
            if(list.statusList == statusList.Deleted) return TempRes.result(401, Request.GetDisplayUrl(), null, "List Tidak Aktif");

            return TempRes.result(200, Request.GetDisplayUrl(), list, null);
        }

        [HttpPut("Delete/{idList}")]
        public async Task<IActionResult> delete(long idList)
        {
            var listdelete = await _listQ.updateStatus(idList, statusList.Deleted);
            if(listdelete == null) return TempRes.result(404, Request.GetDisplayUrl(), listdelete, "Data List Tidak Ditemukan");

            return TempRes.result(200, Request.GetDisplayUrl(), listdelete);
        }

        [HttpPut("Undelete/{idList}")]
        public async Task<IActionResult> undelete(long idList)
        {
            var listdelete = await _listQ.updateStatus(idList, statusList.Active);
            if(listdelete == null) return TempRes.result(404, Request.GetDisplayUrl(), listdelete, "Data List Tidak Ditemukan");

            return TempRes.result(200, Request.GetDisplayUrl(), listdelete);
        }
    }
}