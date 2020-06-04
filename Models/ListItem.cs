using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoListRecruitment.Utility;
using ToDoListRecruitment.Enums;

namespace ToDoListRecruitment.Models
{
    public class ListItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long idListItem{get;set;}
        [Required]
        public string nameListItem{get;set;}
        public string descListItem{get;set;}
        [Required]
        public isDoneListItem isDoneListItem{get;set;}
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime created{get;set;}
        // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime updated{get;set;}

        [Required]
        public long idList{get;set;}
        
        public object this[string propertyName]
        {
            get { 
                try{
                    return this.GetType().GetProperty(propertyName).GetValue(this, null);
                }catch{
                    return null;
                }
            }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }

        public class Query {
            private readonly ApiDb _db;
            public Query (ApiDb db){
                _db = db;
            }

            // MODELS DATA MANIPULATION HERE
            public async Task<ListItem> getById(long id)
            {
                var ListItem = await _db.ListItems.FirstOrDefaultAsync(g => g.idListItem == id);
                if(ListItem == null) return null;
                return ListItem;
            }
            
            public ListItem getByIdSync(long id)
            {
                var ListItem = _db.ListItems.FirstOrDefault(g => g.idListItem == id);
                if(ListItem == null) return null;
                return ListItem;
            }

            public async Task<List<ListItem>> getAll(int page = 1, int size = 10, string key = "", string order = "updated", bool ascending = false, isDoneListItem isDoneListItem = isDoneListItem.Undefined, long idList = 0){
                var q = _db.ListItems
                    .Where(a => a.idListItem == a.idListItem);
                if(!string.IsNullOrEmpty(key)) 
                    q = q.Where(a => 
                        a.nameListItem.Contains(key)
                    ); 
                if(idList != 0) q = q.Where(a => a.idList == idList);
                if(isDoneListItem != isDoneListItem.Undefined) q = q.Where(a => a.isDoneListItem == isDoneListItem);
                switch(order)
                {
                    case "idListItem": if(ascending) q = q.OrderBy(d => d.idListItem); else q = q.OrderByDescending(d => d.idListItem); break;
                    case "nameListItem": if(ascending) q = q.OrderBy(d => d.nameListItem); else q = q.OrderByDescending(d => d.nameListItem); break;
                    case "isDoneListItem": if(ascending) q = q.OrderBy(d => d.isDoneListItem); else q = q.OrderByDescending(d => d.isDoneListItem); break;
                    case "created": if(ascending) q = q.OrderBy(d => d.created); else q = q.OrderByDescending(d => d.created); break;
                    case "updated": if(ascending) q = q.OrderBy(d => d.updated); else q = q.OrderByDescending(d => d.updated); break;
                }

                if(size == -1) return await q
                    .ToListAsync();
                else return await q
                    .Skip(size * (page-1))
                    .Take(size)
                    .ToListAsync();
            }

            public async Task<int> getAllCount(string key = "", string order = "updated", bool ascending = false, isDoneListItem isDoneListItem = isDoneListItem.Undefined, long idList = 0){
                var q = _db.ListItems
                    .Where(a => a.idListItem == a.idListItem);
                if(!string.IsNullOrEmpty(key)) 
                    q = q.Where(a => 
                        a.nameListItem.Contains(key)
                    ); 
                if(idList != 0) q = q.Where(a => a.idList == idList);
                if(isDoneListItem != isDoneListItem.Undefined) q = q.Where(a => a.isDoneListItem == isDoneListItem);
                switch(order)
                {
                    case "idListItem": if(ascending) q = q.OrderBy(d => d.idListItem); else q = q.OrderByDescending(d => d.idListItem); break;
                    case "nameListItem": if(ascending) q = q.OrderBy(d => d.nameListItem); else q = q.OrderByDescending(d => d.nameListItem); break;
                    case "isDoneListItem": if(ascending) q = q.OrderBy(d => d.isDoneListItem); else q = q.OrderByDescending(d => d.isDoneListItem); break;
                    case "created": if(ascending) q = q.OrderBy(d => d.created); else q = q.OrderByDescending(d => d.created); break;
                    case "updated": if(ascending) q = q.OrderBy(d => d.updated); else q = q.OrderByDescending(d => d.updated); break;
                }

                return await q
                    .CountAsync();
            }

            public async Task<ListItem> insert(ListItemInsert ListItem)
            {
                ListItem newListItem = new ListItem()
                {
                    nameListItem = ListItem.nameListItem,
                    descListItem = ListItem.descListItem,
                    isDoneListItem = isDoneListItem.Undone,
                    idList = ListItem.idList,

                    created = DateTime.Now,
                    updated = DateTime.Now
                };

                await _db.AddAsync(newListItem);
                await _db.SaveChangesAsync();
                return newListItem;
            }

            public async Task<ListItem> update(ListItemUpdate ListItem)
            {
                var updateListItem = await _db.ListItems.FirstOrDefaultAsync(a => a.idListItem == ListItem.idListItem);
                if(updateListItem == null) return null;

                updateListItem.nameListItem = string.IsNullOrEmpty(ListItem.nameListItem) ? updateListItem.nameListItem : ListItem.nameListItem;
                updateListItem.descListItem = string.IsNullOrEmpty(ListItem.descListItem) ? updateListItem.descListItem : ListItem.descListItem;

                updateListItem.updated = DateTime.Now;

                await _db.SaveChangesAsync();
                return updateListItem;
            }

            public async Task<ListItem> updateStatus(long idListItem, isDoneListItem isDoneListItem)
            {
                var updateListItem = await _db.ListItems.FirstOrDefaultAsync(a => a.idListItem == idListItem);
                if(updateListItem == null) return null;

                updateListItem.isDoneListItem = isDoneListItem == isDoneListItem.Undefined ? updateListItem.isDoneListItem : isDoneListItem;

                updateListItem.updated = DateTime.Now;

                await _db.SaveChangesAsync();
                return updateListItem;
            }

            public async Task<ListItem> toggleStatus(long idListItem)
            {
                var updateListItem = await _db.ListItems.FirstOrDefaultAsync(a => a.idListItem == idListItem);
                if(updateListItem == null) return null;

                updateListItem.isDoneListItem = updateListItem.isDoneListItem == isDoneListItem.Undone ? isDoneListItem.Done : isDoneListItem.Undone;

                updateListItem.updated = DateTime.Now;

                await _db.SaveChangesAsync();
                return updateListItem;
            }

            public async Task<ListItem> delete(long idListItem)
            {
                var listitemdelete = await _db.ListItems.FirstOrDefaultAsync(a => a.idListItem == idListItem);
                if(listitemdelete == null) return null;

                _db.Remove(listitemdelete);
                await _db.SaveChangesAsync();
                return listitemdelete;

            }
        }
    }

    public class ListItemInsert
    {
        [Required(ErrorMessage = "List Item Wajib Diisi")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Panjang List Item Harus Antara 3 dan 150 Karakter")]
        public string nameListItem{get;set;}
        public string descListItem{get;set;}
        [ListExist(ErrorMessage = "List Tidak Ditemukan")]
        public long idList{get;set;}

        public object this[string propertyName]
        {
            get { 
                try{
                    return this.GetType().GetProperty(propertyName).GetValue(this, null);
                }catch{
                    return null;
                }
            }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }
    }

    public class ListItemUpdate
    {
        [Range(1, long.MaxValue, ErrorMessage = "Id List Item Tidak Boleh Kosong")]
        public long idListItem{get;set;}
        [StringLengthNullable(3, 150, ErrorMessage = "Panjang Nama List Harus Antara 3 dan 150 Karakter")]
        public string nameListItem{get;set;}
        public string descListItem{get;set;}

        public object this[string propertyName]
        {
            get { 
                try{
                    return this.GetType().GetProperty(propertyName).GetValue(this, null);
                }catch{
                    return null;
                }
            }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }
    }
}