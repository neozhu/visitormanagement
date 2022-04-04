// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Blazor.Application.Features.CheckinPoints.DTOs;
using CleanArchitecture.Blazor.Application.Features.CheckinPoints.Caching;
namespace CleanArchitecture.Blazor.Application.Features.CheckinPoints.Commands.AddEdit;

    public class AddEditCheckinPointCommand: CheckinPointDto,IRequest<Result<int>>, IMapFrom<CheckinPoint>, ICacheInvalidator
    {
      public string CacheKey => CheckinPointCacheKey.GetAllCacheKey;
      public CancellationTokenSource? SharedExpiryTokenSource => CheckinPointCacheKey.SharedExpiryTokenSource;
    }

    public class AddEditCheckinPointCommandHandler : IRequestHandler<AddEditCheckinPointCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditCheckinPointCommandHandler> _localizer;
        public AddEditCheckinPointCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditCheckinPointCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(AddEditCheckinPointCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditCheckinPointCommandHandler method 
            if (request.Id > 0)
            {
                var item = await _context.CheckinPoints.FindAsync(new object[] { request.Id }, cancellationToken);
                _ = item ?? throw new NotFoundException("CheckinPoint {request.Id} Not Found.");
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
            else
            {
                var item = _mapper.Map<CheckinPoint>(request);
                _context.CheckinPoints.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
           
        }
    }

