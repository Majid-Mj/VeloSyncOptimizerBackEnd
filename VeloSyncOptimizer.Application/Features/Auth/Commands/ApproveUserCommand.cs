using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeloSyncOptimizer.Application.Features.Auth.Commands;

public record ApproveUserCommand(Guid UserId) : IRequest;
