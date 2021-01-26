﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using Mine.Models;

namespace Mine.Services
{
	public class DatabaseService : IDataStore<ItemModel>
	{
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public DatabaseService()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(ItemModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(ItemModel)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

		public Task<bool> CreateAsync(ItemModel item)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateAsync(ItemModel item)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteAsync(string id)
		{
			throw new NotImplementedException();
		}

		public Task<ItemModel> ReadAsync(string id)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns all items in the database
		/// </summary>
		/// <param name="forceRefresh"></param>
		/// <returns>A list containing all items in database</returns>
		public async Task<IEnumerable<ItemModel>> IndexAsync(bool forceRefresh = false)
		{
			// Get all items from database
			var result = await Database.Table<ItemModel>().ToListAsync();
			return result;
		}
	}
}
