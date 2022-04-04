// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Commands.Update;

    public class UpdateVisitorCommand: VisitorDto,IRequest<Result>, IMapFrom<Visitor>, ICacheInvalidator
    {
        public string CacheKey => VisitorCacheKey.GetAllCacheKey;
        public CancellationTokenSource? SharedExpiryTokenSource => VisitorCacheKey.SharedExpiryTokenSource;
    }

    public class UpdateVisitorCommandHandler : IRequestHandler<UpdateVisitorCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateVisitorCommandHandler> _localizer;
        public UpdateVisitorCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateVisitorCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateVisitorCommand request, CancellationToken cancellationToken)
        {
           
           var item =await _context.Visitors.FindAsync( new object[] { request.Id }, cancellationToken);
           if (item != null)
           {
                item = _mapper.Map(request, item);
            var updateevent=new VisitorUpdatedEvent(item);
            item.DomainEvents.Add(updateevent);
                await _context.SaveChangesAsync(cancellationToken);
           }
           return Result.Success();
        }
    }

