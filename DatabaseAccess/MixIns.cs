using DatabaseAccess.DatabaseTables;
using NHibernate;


namespace DatabaseAccess
{
    public static class IModelMixIn
    {
        public static void Refresh(this IModel model)
        {
            model.Session.Refresh(model.Under);
        }

        public static bool Update(this IModel model)
        {
            using (ITransaction transaction = model.Session.BeginTransaction())
            {

                model.Session.SaveOrUpdate(model.Under);
                model.ExtraUpdate();

                transaction.Commit();

                return transaction.WasCommitted;
            }
        }

        public static bool Delete(this IModel model)
        {
            using (ITransaction transaction = model.Session.BeginTransaction())
            {
                model.ExtraDelete();
                model.Session.Delete(model.Under);

                transaction.Commit();

                return transaction.WasCommitted;
            }

        }
    }
}




