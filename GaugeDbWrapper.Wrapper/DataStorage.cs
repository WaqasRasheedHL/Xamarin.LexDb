using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lex.Db;
namespace GaugeDbWrapper.Wrapper
{
    /// <summary>
    /// This class is used for database 
    /// processing locally.
    /// </summary>
    public class DataStorage
    {
        #region 'Constructor'
        private DataStorage()
        {
            try
            {
                databaseDbInstance = new DbInstance(DatabaseName);
                InitializeLexDb();
            }
            catch (Exception exception)
            {
                throw;
            }

        }

        #endregion

        #region 'Properties'
        private static readonly object LockSync = new object();
        private readonly DbInstance databaseDbInstance;
        private const string DatabaseName = "LexDbGauge";
        private static DataStorage dBManagerInstance;
        /// <summary>
        /// Singleton Pattern is implemented here.
        /// This property returns the same instance
        /// against every call.
        /// </summary>
        public static DataStorage DbManagerInstance
        {
            get
            {
                lock (LockSync)
                {
                    if (dBManagerInstance == null)
                        dBManagerInstance = new DataStorage();
                }
                return dBManagerInstance;
            }

        }

        #endregion

        #region 'Functions'
        /// <summary>
        /// This method initializes all the tables
        /// for local database
        /// </summary>
        private void InitializeLexDb()
        {
            try
            {
                if (!databaseDbInstance.HasMap<ClassNameHere>())
                    databaseDbInstance.Map<ClassNameHere>().Automap(i => i.Id, false);

                // If we want to use only a few attributes from a class
                if (!databaseDbInstance.HasMap<ClassWithAttributes>())
                {
                    databaseDbInstance.Map<ClassWithAttributes>()
                        .Key(i => i.Id, false)
                        .Map(i => i.FirstName)
                        .Map(i => i.LastName)
                        .Map(i => i.Address1);
                }
                databaseDbInstance.Initialize();

            }
            catch (Exception exception)
            {
                throw new Exception(Constants.DbConstants.DbInitializationError);
            }

        }
        public ClassWithAttributes GetUserById(int userId)
        {
            try
            {
                var userdatabase = new GenericRepository<ClassWithAttributes>(databaseDbInstance);
                var user = userdatabase.GetById(userId);
                return user;
            }
            catch (Exception exception)
            {
                throw new Exception(Constants.DbConstants.DbServicesError);
            }

        }
        public bool SaveUser(List<ClassWithAttributes> userList)
        {
            try
            {   
                var userDatabase = new GenericRepository<ClassWithAttributes>(databaseDbInstance);
				foreach (var user in userList)
					user.Id = userDatabase.GetMaxKey();
                userDatabase.Insert(userList); // Single user can also be saved
                return true;
            }
            catch (Exception exception)
            {
                return false;
            }

        }

        public bool DeleteUser(ClassWithAttributes user)
        {
            try
            {
                var userDatabase = new GenericRepository<ClassWithAttributes>(databaseDbInstance);
                userDatabase.Delete(user);
                return true;
            }
            catch (Exception exception)
            {
                return false;
            }

        }
        public IEnumerable<ClassWithAttributes> GetAllUsers()
        {
            var userDatabase = new GenericRepository<ClassWithAttributes>(databaseDbInstance);
            var userList = userDatabase.Get();
            return userList;
        }


        // Model Classes declaring here, Just for Sample
        public class ClassNameHere
        {
            public int Id { get; set; }
            public string Origin { get; set; }
        }
        public class ClassWithAttributes
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
        }

        #endregion
    }
}
