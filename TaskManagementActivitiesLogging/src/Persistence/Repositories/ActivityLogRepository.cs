using Application.DTO.ActivityLog;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using Persistence.Database.Context;

namespace Persistence.Repositories
{
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public ActivityLogRepository(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public async Task CreateAsync<T>(T request) where T : CreateActivityLog
        {
            var entity = _mapper.Map<ActivityLog>(request);

            await using var dbContext = _dbContextFactory.CreateDbContext();

            var result = dbContext.ActivityLogs.Add(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var entity = await dbContext.ActivityLogs.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException($"{nameof(ActivityLog)} with id {id} not found");
            }

            var result = dbContext.ActivityLogs.Remove(entity);

            await dbContext.SaveChangesAsync();
        }

        public async Task<List<ActivityLogResponse>> GetAsync(FilteringActivityLogRequest request)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var query = BuildQueryForFilteringActivityLog(request, dbContext);

            var entities = await query.ToListAsync();

            return _mapper.Map<List<ActivityLogResponse>>(entities);
        }

        private IQueryable<ActivityLog> BuildQueryForFilteringActivityLog(FilteringActivityLogRequest request, DataBaseContext dbContext)
        {
            var query = dbContext.ActivityLogs.AsQueryable();

            if (request.Ids is not null && request.Ids.Length > 0)
            {
                query = query.Where(x => request.Ids.Contains(x.Id));
            }

            if (!string.IsNullOrEmpty(request.EventType))
            {
                query = query.Where(x => EF.Functions.ILike(x.EventType, $"%{request.EventType}%"));
            }

            if (!string.IsNullOrEmpty(request.Entity))
            {
                query = query.Where(x => EF.Functions.ILike(x.Entity, $"%{request.Entity}%"));
            }

            if (request.EntityId.HasValue)
            {
                query = query.Where(x => x.EntityId == request.EntityId);
            }

            if (request.EventTimeStart.HasValue)
            {
                query = query.Where(x => x.EventTime >= request.EventTimeStart);
            }

            if (request.EventTimeEnd.HasValue)
            {
                query = query.Where(x => x.EventTime <= request.EventTimeEnd);
            }

            return query;
        }

        public async Task<bool> ExistsById(long id)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            return await dbContext.ActivityLogs.AnyAsync(x => x.Id == id);
        }
    }
}
