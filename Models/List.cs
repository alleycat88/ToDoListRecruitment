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
    public class List
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long idList{get;set;}
        [Required]
        public string nameList{get;set;}
        [Required]
        public string colorHexList{get;set;}
        [Required]
        public statusList statusList{get;set;}
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime created{get;set;}
        // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime updated{get;set;}

        [ForeignKey("idList")]
        public ICollection<ListItem> listItems{get;set;}
        
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
            public async Task<List> getById(long id)
            {
                var List = await _db.Lists.FirstOrDefaultAsync(g => g.idList == id);
                if(List == null) return null;
                return List;
            }
            
            public List getByIdSync(long id)
            {
                var List = _db.Lists.FirstOrDefault(g => g.idList == id);
                if(List == null) return null;
                return List;
            }

            public async Task<List> getDetail(long id)
            {
                var List = await _db.Lists.Include(a => a.listItems).FirstOrDefaultAsync(g => g.idList == id);
                if(List == null) return null;
                return List;
            }

            public async Task<List<List>> getAll(int page = 1, int size = 10, string key = "", string order = "updated", bool ascending = false, statusList statusList = statusList.Active){
                var q = _db.Lists
                    .Where(a => a.idList == a.idList);
                if(!string.IsNullOrEmpty(key)) 
                    q = q.Where(a => 
                        a.nameList.Contains(key)
                    ); 
                if(statusList != statusList.Undefined) q = q.Where(a => a.statusList == statusList);
                switch(order)
                {
                    case "idList": if(ascending) q = q.OrderBy(d => d.idList); else q = q.OrderByDescending(d => d.idList); break;
                    case "nameList": if(ascending) q = q.OrderBy(d => d.nameList); else q = q.OrderByDescending(d => d.nameList); break;
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

            public async Task<int> getAllCount(string key = "", string order = "updated", bool ascending = false, statusList statusList = statusList.Undefined){
                var q = _db.Lists
                    .Where(a => a.idList == a.idList);
                if(!string.IsNullOrEmpty(key)) 
                    q = q.Where(a => 
                        a.nameList.Contains(key)
                    ); 
                if(statusList != statusList.Undefined) q = q.Where(a => a.statusList == statusList);
                switch(order)
                {
                    case "idList": if(ascending) q = q.OrderBy(d => d.idList); else q = q.OrderByDescending(d => d.idList); break;
                    case "nameList": if(ascending) q = q.OrderBy(d => d.nameList); else q = q.OrderByDescending(d => d.nameList); break;
                    case "created": if(ascending) q = q.OrderBy(d => d.created); else q = q.OrderByDescending(d => d.created); break;
                    case "updated": if(ascending) q = q.OrderBy(d => d.updated); else q = q.OrderByDescending(d => d.updated); break;
                }

                return await q
                    .CountAsync();
            }

            public async Task<List> insert(ListInsert List)
            {
                List newList = new List()
                {
                    nameList = List.nameList,
                    colorHexList = string.IsNullOrEmpty(List.colorHexList) ? "#ffffff" : List.colorHexList,
                    statusList = statusList.Active,

                    created = DateTime.Now,
                    updated = DateTime.Now
                };

                await _db.AddAsync(newList);
                await _db.SaveChangesAsync();
                return newList;
            }

            public async Task<List> update(ListUpdate List)
            {
                var updateList = await _db.Lists.FirstOrDefaultAsync(a => a.idList == List.idList);
                if(updateList == null) return null;

                updateList.nameList = string.IsNullOrEmpty(List.nameList) ? updateList.nameList : List.nameList;
                updateList.colorHexList = string.IsNullOrEmpty(List.colorHexList) ? updateList.colorHexList : List.colorHexList;

                updateList.updated = DateTime.Now;

                await _db.SaveChangesAsync();
                return updateList;
            }

            public async Task<List> updateStatus(long idList, statusList statusList)
            {
                var updateList = await _db.Lists.FirstOrDefaultAsync(a => a.idList == idList);
                if(updateList == null) return null;

                updateList.statusList = statusList == statusList.Undefined ? updateList.statusList : statusList;

                updateList.updated = DateTime.Now;

                await _db.SaveChangesAsync();
                return updateList;
            }
        }
    }

    public class ListInsert
    {
        [Required(ErrorMessage = "Nama List Wajib Diisi")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Panjang Nama List Harus Antara 3 dan 150 Karakter")]
        public string nameList{get;set;}
        [ColorHex(ErrorMessage = "Hex Warna Tidak Valid")]
        public string colorHexList{get;set;}

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

    public class ListUpdate
    {
        [Range(0, long.MaxValue, ErrorMessage = "Id List Tidak Boleh Kosong")]
        public long idList{get;set;}
        [StringLengthNullable(3, 150, ErrorMessage = "Nama List Harus Antara 3 dan 150 Karakter")]
        public string nameList{get;set;}
        [ColorHex(ErrorMessage = "Hex Warna Tidak Valid")]
        public string colorHexList{get;set;}

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