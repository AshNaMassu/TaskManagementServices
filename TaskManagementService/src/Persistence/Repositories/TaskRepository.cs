using Application.DTO.Task;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using Persistence.Database.Context;

namespace Persistence.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public TaskRepository(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public async Task<long> CreateAsync(CreateTaskRequest request)
        {
            var entity = _mapper.Map<TaskEntity>(request);

            await using var dbContext = _dbContextFactory.CreateDbContext();

            await dbContext.Tasks.AddAsync(entity);
            await dbContext.SaveChangesAsync();

            return entity.Id;
        }

        public async Task DeleteAsync(long id)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var entity = await dbContext.Tasks.FindAsync(id);

            if (entity is null)
            {
                throw new NotFoundException($"{nameof(TaskEntity)} with id {id} not found");
            }

            dbContext.Tasks.Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsById(long id)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            return await dbContext.Tasks.AnyAsync(x => x.Id == id);
        }

        public async Task<List<TaskResponse>> GetAsync(FilteringTaskRequest request)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var query = BuildQueryForFilteringTask(request, dbContext);

            var entities = await query.ToListAsync();

            return _mapper.Map<List<TaskResponse>>(entities);
        }

        public async Task<TaskResponse> GetAsync(long id)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var entity = await dbContext.Tasks.FindAsync(id);

            if (entity is null)
            {
                throw new NotFoundException($"{nameof(TaskEntity)} with id {id} not found");
            }

            return _mapper.Map<TaskResponse>(entity);
        }

        public async Task UpdateAsync(long id, UpdateTaskRequest updateTaskRequest)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var entity = await dbContext.Tasks.FindAsync(id);

            if (entity is null)
            {
                throw new NotFoundException($"{nameof(TaskEntity)} with id {id} not found");
            }

            entity.Title = updateTaskRequest.Title;
            entity.Description = updateTaskRequest.Description;
            entity.Status = updateTaskRequest.Status;
            entity.UpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(long id, UpdateTaskStatusRequest updateTaskRequest)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var entity = await dbContext.Tasks.FindAsync(id);

            if (entity is null)
            {
                throw new NotFoundException($"{nameof(TaskEntity)} with id {id} not found");
            }

            entity.Status = updateTaskRequest.Status;
            entity.UpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
        }

        private IQueryable<TaskEntity> BuildQueryForFilteringTask(FilteringTaskRequest request, DataBaseContext dbContext)
        {
            var query = dbContext.Tasks.AsQueryable();

            if (request.Ids is not null && request.Ids.Length > 0)
            {
                query = query.Where(x => request.Ids.Contains(x.Id));
            }

            if (!string.IsNullOrEmpty(request.Title))
            {
                query = query.Where(x => EF.Functions.ILike(x.Title, $"%{request.Title}%"));
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                query = query.Where(x => EF.Functions.ILike(x.Description, $"%{request.Description}%"));
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                query = query.Where(x => x.Status == request.Status);
            }

            if (request.CreatedAtStart.HasValue)
            {
                query = query.Where(x => x.CreatedAt >= request.CreatedAtStart);
            }

            if (request.CreatedAtEnd.HasValue)
            {
                query = query.Where(x => x.CreatedAt <= request.CreatedAtEnd);
            }

            if (request.UpdatedAtStart.HasValue)
            {
                query = query.Where(x => x.UpdatedAt >= request.UpdatedAtStart);
            }

            if (request.UpdatedAtEnd.HasValue)
            {
                query = query.Where(x => x.UpdatedAt <= request.UpdatedAtEnd);
            }

            query = query.OrderBy(x => x.Id)
                         .Skip(request.Offset)
                         .Take(request.Limit);

            return query;
        }
    }
}
