using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mine.Models;

namespace Mine.Services
{
    public class MockDataStore : IDataStore<ItemModel>
    {
        readonly List<ItemModel> items;

        public MockDataStore()
        {
            items = new List<ItemModel>()
            {
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Rumbling rock", Description="Creates the illusion of a tumbling rock to scare any opponents (but in fact it's just hungry).", Value=3 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Snowshoes", Description="Keeps you afloat on fluffy stuff.", Value=2 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Beauceron", Description="When you need a snuggle break, get your Beauceron.", Value=8 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Super caffeinated coffee", Description="Energy booster very different from normal coffee.", Value=5 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Homemade bread", Description="Causes any bystander to fall into trance.", Value=7},
            };
        }

        public async Task<bool> CreateAsync(ItemModel item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync(ItemModel item)
        {
            var oldItem = items.Where((ItemModel arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var oldItem = items.Where((ItemModel arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<ItemModel> ReadAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<ItemModel>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}