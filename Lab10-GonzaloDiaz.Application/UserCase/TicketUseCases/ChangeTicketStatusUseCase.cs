    using Lab10_GonzaloDiaz.Application.Dtos;
    using Lab10_GonzaloDiaz.Domain.Ports;

    namespace Lab10_GonzaloDiaz.Application.UserCase.TicketUseCase;

    public class ChangeTicketStatusUseCase
    {
        private readonly ITicketRepository _ticketRepository;

        public ChangeTicketStatusUseCase(
            ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<TicketDto> Execute(
            Guid id,
            ChangeTicketStatusDto dto)
        {
            if (dto.Status != "abierto" &&
                dto.Status != "en_proceso" &&
                dto.Status != "cerrado")
            {
                throw new ArgumentException(
                    "El estado debe ser abierto, en_proceso o cerrado.");
            }

            var ticket = await _ticketRepository.GetByIdAsync(id);

            if (ticket == null)
            {
                throw new KeyNotFoundException("El ticket no existe.");
            }
            

            _ticketRepository.Update(ticket);
            await _ticketRepository.SaveChangesAsync();

            return new TicketDto
            {
                TicketId = ticket.TicketId,
                UserId = ticket.UserId,
                Title = ticket.Title,
                Description = ticket.Description,
                CreatedAt = ticket.CreatedAt,
                ClosedAt = ticket.ClosedAt
            };
        }
    }