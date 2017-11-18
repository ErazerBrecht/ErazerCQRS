﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Erazer.Services.Queries.DTOs;

namespace Erazer.Services.Queries.Repositories
{
    public interface IPriorityQueryRepository
    {
        Task<IList<PriorityDto>> All();
        Task<PriorityDto> Find(string id);
        Task<bool> Any();

        Task Add(PriorityDto status);
    }
}                                                                                                                    