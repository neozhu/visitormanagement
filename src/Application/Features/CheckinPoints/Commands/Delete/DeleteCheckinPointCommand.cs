// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.CheckinPoints.DTOs;
using CleanArchitecture.Blazor.Application.Features.CheckinPoints.Caching;


namespace CleanArchitecture.Blazor.Application.Features.CheckinPoints.Commands.Delete;

    public class DeleteCheckinPointCommand: IRequest<Result>, ICacheInvalidator
    {
      public int[] Id {  get; }
      public string CacheKey => CheckinPointCacheKey.GetAllCacheKey;
      public CancellationTokenSource? SharedExpiryTokenSource => CheckinPointCacheKey.SharedExpiryTokenSource;
      public DeleteCheckinPointCommand(int[] id)
      {
        Id = id;
      }
    }

    public class DeleteCheckinPointCommandHandler : 
                 IRequestHandler<DeleteCheckinPointCommand, Result>

    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteCheckinPointCommandHandler> _localizer;
        public DeleteCheckinPointCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteCheckinPointCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteCheckinPointCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing DeleteCheckedCheckinPointsCommandHandler method 
            var items = await _context.CheckinPoints.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.CheckinPoints.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

    }

