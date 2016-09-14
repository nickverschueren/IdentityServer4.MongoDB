﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.MongoDB.Interfaces;
using IdentityServer4.MongoDB.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace IdentityServer4.MongoDB.Stores
{
    public class ScopeStore : IScopeStore
    {
        private readonly IConfigurationDbContext context;

        public ScopeStore(IConfigurationDbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            this.context = context;
        }

        public Task<IEnumerable<Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
        {
            IQueryable<Entities.Scope> scopes = context.Scopes;

            if (scopeNames != null && scopeNames.Any())
            {
                scopes = scopes.Where(x => scopeNames.Contains(x.Name));
            }

            var foundScopes = scopes.ToList();
            var model = foundScopes.Select(x => x.ToModel());

            return Task.FromResult(model);
        }

        public Task<IEnumerable<Scope>> GetScopesAsync(bool publicOnly = true)
        {
            IQueryable<Entities.Scope> scopes = context.Scopes;

            if (publicOnly)
            {
                scopes = scopes.Where(x => x.ShowInDiscoveryDocument);
            }

            var foundScopes = scopes.ToList();
            var model = foundScopes.Select(x => x.ToModel());

            return Task.FromResult(model);
        }
    }
}