using System;
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

		/// <summary>
		/// Inserts (creates) an item into the database. 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public async Task<bool> CreateAsync(ItemModel item)
		{
			// Cannot add a null item to database
			if (item == null)
			{
				return false;
			}

			// Insert item and check if insert was succesful. 
			var result = await Database.InsertAsync(item);
			if (result == 0)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Updates the specified item in the database. 
		/// </summary>
		/// <param name="item">Item to update</param>
		/// <returns>True if update was successful, false if not.</returns>
		public async Task<bool> UpdateAsync(ItemModel item)
		{
			// Cannot update a null item. 
			if (item == null)
			{
				return false;
			}

			// Perform update and check if successful
			var result = await Database.UpdateAsync(item);
			if( result == 0)
			{
				return false;
			}

			return true;
		}

		public Task<bool> DeleteAsync(string id)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads an item from the database. 
		/// </summary>
		/// <param name="id">id of item to retrieve</param>
		/// <returns>True if item was found, false if not.</returns>
		public Task<ItemModel> ReadAsync(string id)
		{
			// Cannot search for a null id in database
			if (id == null)
			{
				return null;
			}

			// Call the databasbe to read the ID. 
			// Using Linq syntax. Find the first record that has the ID that matches.
			var result = Database.Table<ItemModel>().FirstOrDefaultAsync(mbox => mbox.Id.Equals(id));

			return result;
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
