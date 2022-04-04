// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Employees.DTOs;
using CleanArchitecture.Blazor.Application.Features.Employees.Caching;


namespace CleanArchitecture.Blazor.Application.Features.Employees.Commands.Delete;

    public class DeleteEmployeeCommand: IRequest<Result>, ICacheInvalidator
    {
      public int[] Id {  get; }
      public string CacheKey => EmployeeCacheKey.GetAllCacheKey;
      public CancellationTokenSource? SharedExpiryTokenSource => EmployeeCacheKey.SharedExpiryTokenSource;
      public DeleteEmployeeCommand(int[] id)
      {
        Id = id;
      }
    }

    public class DeleteEmployeeCommandHandler : 
                 IRequestHandler<DeleteEmployeeCommand, Result>

    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteEmployeeCommandHandler> _localizer;
        public DeleteEmployeeCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteEmployeeCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
           
            var items = await _context.Employees.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
            var deleteevent = new EmployeeDeletedEvent(item);
            item.DomainEvents.Add(deleteevent);
                _context.Employees.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

    }

