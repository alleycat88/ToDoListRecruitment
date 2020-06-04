using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ToDoListRecruitment.Enums;
using ToDoListRecruitment.Models;

namespace ToDoListRecruitment.Controllers.API
{
    [Route("api/[controller]")]
    public class ListItemController : Controller
    {
        private readonly ApiDb _db;
        private ListItem.Query _listItemQ;

        public ListItemController(ApiDb db)
        {
            _db = db;
            _listItemQ = new ListItem.Query(db);
        }

        [HttpPost]
        public async Task<IActionResult> post([FromBody] ListItemInsert listItem)
        {
            if (listItem == null) return TempRes.result(400, Request.GetDisplayUrl(), listItem, "Data List Item Tidak Boleh Kosong");

            // MODELSTATE BASED VALIDATION
            ModelState.Clear();
            TryValidateModel(listItem);
            if (!ModelState.IsValid)
            {
                return TempRes.result(400, Request.GetDisplayUrl(), listItem, ModelState.Values.SelectMany(ms => ms.Errors).FirstOrDefault().ErrorMessage);
            }

            var newlistItem = await _listItemQ.insert(listItem);

            return TempRes.result(200, Request.GetDisplayUrl(), newlistItem);
        }

        [HttpPut]
        public async Task<IActionResult> put([FromBody] ListItemUpdate listItem)
        {
            if (listItem == null) return TempRes.result(400, Request.GetDisplayUrl(), listItem, "Data List Item Tidak Boleh Kosong");

            // MODELSTATE BASED VALIDATION
            ModelState.Clear();
            TryValidateModel(listItem);
            if (!ModelState.IsValid)
            {
                return TempRes.result(400, Request.GetDisplayUrl(), listItem, ModelState.Values.SelectMany(ms => ms.Errors).FirstOrDefault().ErrorMessage);
            }

            var listItemupdate = await _listItemQ.update(listItem);
            if (listItemupdate == null) return TempRes.result(404, Request.GetDisplayUrl(), listItem, "Data List Item Tidak Ditemukan");

            return TempRes.result(200, Request.GetDisplayUrl(), listItemupdate);
        }

        [HttpPut("ToggleStatus/{idListItem}")]
        public async Task<IActionResult> toggleStatus(long idListItem)
        {
            var listItemupdate = await _listItemQ.toggleStatus(idListItem);
            if (listItemupdate == null) return TempRes.result(404, Request.GetDisplayUrl(), null, "Data List Item Tidak Ditemukan");

            return TempRes.result(200, Request.GetDisplayUrl(), listItemupdate);
        }

        [HttpDelete("Delete/{idListItem}")]
        public async Task<IActionResult> delete(long idListItem)
        {
            var listItemdelete = await _listItemQ.delete(idListItem);
            if(listItemdelete == null) return TempRes.result(404, Request.GetDisplayUrl(), listItemdelete, "Data List Item Tidak Ditemukan");

            return TempRes.result(200, Request.GetDisplayUrl(), listItemdelete);
        }
    }
}