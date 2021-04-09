/*
 * Program : Clash Of SL Server
 * Description : A C# Writted 'Clash of SL' Server Emulator !
 *
 * Authors:  Sky Tharusha <Founder at Sky Production>,
 *           And the Official DARK Developement Team
 *
 * Copyright (c) 2021  Sky Production
 * All Rights Reserved.
 */

using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace CSS.Database
{
    internal class cssdbEntities : DbContext
    {
        #region Public Constructors

        public cssdbEntities(string connectionString) : base("name=" + connectionString)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        #endregion Protected Methods

        #region Public Properties

        public virtual DbSet<clan> clan { get; set; }

        public virtual DbSet<player> player { get; set; }

        #endregion Public Properties
    }
}