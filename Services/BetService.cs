using BetsApi.Data.Repositories;
using BetsApi.Models;
using BetsApi.Repositories;

namespace BetsApi.Services
{
    public interface IBetService
    {
        Task<Bet> CreateBetAsync(Bet bet);
        Task UpdateBetAsync(long id, Bet bet);
        Task DeleteBetAsync(long id);
        Task<IEnumerable<Bet>> GetAllBetAsync();
        Task<Bet> GetBetAsync(long id);
        Task<Bet> AcceptBetAsync(long id);
        Task<Bet> DeclineBetAsync(long id);
    }

    public class BetService : IBetService
    {
        private readonly IBetRepository _betRepository;
        private readonly IUserRepository _userRepository;

        public BetService(IBetRepository betRepository, IUserRepository userRepository)
        {
            _betRepository = betRepository;
            _userRepository = userRepository;
        }

        public async Task<Bet> AcceptBetAsync(long id)
        {
            var bet = await _betRepository.GetByIdAsync(id);
            if (bet != null && bet.BetState == State.Pending) 
            {
                bet.BetState = State.Accepted;
                await _betRepository.UpdateAsync(bet);
            }
            return bet;
        }

        public async Task<Bet> DeclineBetAsync(long id)
        {
            var bet = await _betRepository.GetByIdAsync(id);
            if (bet != null && bet.BetState == State.Pending)
            {
                bet.BetState = State.Declined;
                await _betRepository.UpdateAsync(bet);
            }
            return bet;
        }

        public async Task DeleteBetAsync(long id)
        {
            var bet = await _betRepository.GetByIdAsync(id);
            if (bet != null)
            {
                var sender = await _userRepository.GetByIdAsync(bet.SenderId);
                var receiver = await _userRepository.GetByIdAsync(bet.ReceiverId);

                sender.BetsSent.Remove(bet);
                receiver.BetsReceived.Remove(bet);
                await _userRepository.UpdateAsync(sender);
                await _userRepository.UpdateAsync(receiver);
            }
            else
            {
                throw new ArgumentException("Bet with this id does not exist");
            }
        }

        public async Task<IEnumerable<Bet>> GetAllBetAsync()
        {
            return await _betRepository.GetAllAsync();
        }

        public async Task<Bet> GetBetAsync(long id)
        {
            return await _betRepository.GetByIdAsync(id);
        }

        public async Task UpdateBetAsync(long id, Bet bet)
        {
            var existingBet = await _betRepository.GetByIdAsync(id);
            if (existingBet != null)
            {
                existingBet.Wager = bet.Wager;
                existingBet.Amount = bet.Amount;
                existingBet.BetState = bet.BetState;
                await _betRepository.UpdateAsync(existingBet);
            }
            else
            {
                throw new InvalidOperationException("Bet not found with Id");
            }
        }

        public async Task<Bet> CreateBetAsync(Bet bet)
        {
            if (bet == null)
            {
                throw new ArgumentNullException(nameof(bet));
            }

            var sender = await _userRepository.GetByIdAsync(bet.SenderId);
            var receiver = await _userRepository.GetByIdAsync(bet.ReceiverId);

            if (sender == null || receiver == null)
            {
                throw new ArgumentException("Invalid sender or receiver ID");
            }

            bet.Sender = sender;
            bet.Receiver = receiver;
            await _betRepository.AddAsync(bet);

            await _userRepository.UpdateAsync(sender);
            await _userRepository.UpdateAsync(receiver);

            return bet;
        }
    }
}
