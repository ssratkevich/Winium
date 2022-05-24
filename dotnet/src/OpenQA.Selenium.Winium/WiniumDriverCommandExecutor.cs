using System;
using OpenQA.Selenium.Remote;

namespace OpenQA.Selenium.Winium
{
    /// <summary>
    /// Winium command executor.
    /// </summary>
    public class WiniumDriverCommandExecutor : ICommandExecutor
    {
        #region Fields

        private ICommandExecutor internalExecutor;

        private WiniumDriverService service;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Creates new instance of command executor.
        /// </summary>
        /// <param name="driverService">Winium service.</param>
        /// <param name="commandTimeout">Timeout for command.</param>
        public WiniumDriverCommandExecutor(WiniumDriverService driverService, TimeSpan commandTimeout)
        {
            this.service = driverService;
            this.internalExecutor = CommandExecutorFactory.GetHttpCommandExecutor(driverService.ServiceUrl, commandTimeout);
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc/>
        public CommandInfoRepository CommandInfoRepository =>
            this.internalExecutor.CommandInfoRepository;

        /// <summary>
        /// Try to add new command.
        /// </summary>
        /// <param name="commandName">Command.</param>
        /// <param name="info">Command info.</param>
        /// <returns>Operation result.</returns>
        public bool TryAddCommand(string commandName, CommandInfo info) =>
            this.internalExecutor.CommandInfoRepository.TryAddCommand(commandName, info);

        /// <inheritdoc/>
        public Response Execute(Command commandToExecute)
        {
            if (commandToExecute == null)
            {
                throw new ArgumentNullException("commandToExecute", "Command may not be null");
            }

            if (commandToExecute.Name == DriverCommand.NewSession)
            {
                this.service.Start();
            }

            try
            {
                return this.internalExecutor.Execute(commandToExecute);
            }
            finally
            {
                if (commandToExecute.Name == DriverCommand.Quit)
                {
                    this.service.Dispose();
                }
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.internalExecutor.Dispose();
        }

        #endregion
    }
}
