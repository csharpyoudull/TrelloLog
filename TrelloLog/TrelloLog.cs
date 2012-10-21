using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using TrelloNet;

namespace TrelloLog
{
    /// <summary>
    /// Class TrelloLog
    /// </summary>
    public class TrelloLog
    {
        /// <summary>
        /// Gets or sets the name of the log board.
        /// </summary>
        /// <value>The name of the log board.</value>
        private string LogBoardName { get; set; }

        /// <summary>
        /// Gets or sets the trello client.
        /// </summary>
        /// <value>The trello client.</value>
        private static Trello TrelloClient { get; set; }

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        /// <value>The organization.</value>
        private static Organization Organization { get; set; }

        /// <summary>
        /// Gets or sets the tasks.
        /// </summary>
        /// <value>The tasks.</value>
        private TaskFactory Tasks { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrelloLog" /> class.
        /// </summary>
        public TrelloLog()
        {
            Tasks = new TaskFactory();
            LogBoardName = ConfigurationManager.AppSettings["Trello-LogBoardName"];

            if (TrelloClient != null)
                return;

            var appKey = ConfigurationManager.AppSettings["Trello-ApplicationKey"];
            var token = ConfigurationManager.AppSettings["Trello-AuthToken"];
            var org = ConfigurationManager.AppSettings["Trello-Organization"];

            var trello = new Trello(appKey);
            trello.Authorize(token);
            TrelloClient = trello;

            if (!string.IsNullOrEmpty(org))
                Organization =
                    TrelloClient.Organizations.ForMe().FirstOrDefault(
                        o => o.Name.Equals(org, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Logs the info.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="info">The info.</param>
        public void LogInfo(string applicationName, string sender, string info)
        {
            var board = GetBoard(LogBoardName);
            var list = GetList(applicationName, board);
            var card = TrelloClient.Cards.Add(new NewCard(sender, list));
            TrelloClient.Cards.AddLabel(card, Color.Green);
            TrelloClient.Cards.AddComment(card, info);

        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="info">The info.</param>
        public void LogWarning(string applicationName, string sender, string info)
        {

            var board = GetBoard(LogBoardName);
            var list = GetList(applicationName, board);
            var card = TrelloClient.Cards.Add(new NewCard(sender, list));
            TrelloClient.Cards.AddLabel(card, Color.Yellow);
            TrelloClient.Cards.AddComment(card, info);
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="ex">The ex.</param>
        public void LogException(string applicationName, string sender, Exception ex)
        {
            var board = GetBoard(LogBoardName);
            var list = GetList(applicationName, board);
            var card = TrelloClient.Cards.Add(new NewCard(sender, list));
            TrelloClient.Cards.AddLabel(card, Color.Red);
            TrelloClient.Cards.AddComment(card, ex.ToString());
        }

        /// <summary>
        /// Gets the board.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Board.</returns>
        public Board GetBoard(string name)
        {
            var boards = Organization != null ? TrelloClient.Boards.ForOrganization(Organization).ToList() : TrelloClient.Boards.ForMe().ToList();

            if (!boards.Any(b => b.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && !b.Closed))
            {
                var newBoard = new NewBoard(name);

                if (Organization != null)
                    newBoard.IdOrganization = Organization.Id;

                var board = TrelloClient.Boards.Add(newBoard);
                var lists = TrelloClient.Lists.ForBoard(board);
                foreach (var list in lists)
                {
                    TrelloClient.Lists.Archive(list);
                }

                return board;
            }

            return boards.FirstOrDefault(b => b.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && !b.Closed);
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="board">The board.</param>
        /// <returns>List.</returns>
        public List GetList(string name, Board board)
        {
            var found =
                TrelloClient.Lists.ForBoard(board).FirstOrDefault(
                    l => l.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && !l.Closed);

            return found ?? TrelloClient.Lists.Add(new NewList(name, board));
        }

    }
}
