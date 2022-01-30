using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaExchangePlatform.Core.Config
{
    public static class Constants
    {
        #region Global Project Values
        public const string companyName = "EVA";
        public const string projectName = "EVA Exchange Platform";
        #endregion

        #region Error Messages
        public const string emptyInput = "Please enter all values completely!";
        public const string inputForbiddenWordError = "The values ​​you entered contain forbidden words";
        public const string traderIdError = "TraderId cannot be null!";
        public const string registerAmountError = "Amount cannot be empty or negative value!";
        public const string shareRegisterError = "Share cannot registered!";

        public const string priceError = "Share price cannot be negative value!";
        public const string portfolioError = "Share should be registered in the trader portfolio!";
        public const string traderBalanceError = "Trader balance is not enough for trading!";
        public const string shareAmountError = "The share amount is not enough for trading!";
        public const string registerShareError = "The share cannot registered!";

        public const string idError = "Id cannot be null!";
        public const string checkRegisteredShareError = "Share is not registered or does not belong to this trader!";
        public const string updateRegisteredShareDateError = "You can only update the price once in hour!";
        public const string updateRegisteredShareError = "The registered share cannot updated!";
        public const string registeredShareIdError = "The registered share ID cannot be null!";

        public const string registeredShareBuySideError = "The selected share is not suitable for BUY!";
        public const string registeredShareSellSideError = "The selected share is not suitable for SELL!";
        public const string amountCheckError = "The amount to be purchased cannot exceed the entire share!";
        public const string checkRegisteredShareRecordError = "No such registered share records were found!";
        public const string amountSellError = "The amount of shares you want to sell cannot be more than trader's own!";
        #endregion

        #region Successful Messages
        public const string registerShareSuccessful = "The share registered successfully!";
        public const string updateShareSuccessful = "The registered share updated successfully!";
        public const string buyTradeSuccessful = "The BUY trade was successful!";
        public const string sellTradeSuccessful = "The SELL trade was successful!";
        #endregion

        #region Trade Sides
        public const string buySide = "BUY";
        public const string sellSide = "SELL";
        #endregion

        #region Log Statuses
        public const string success = "SUCCESSFUL";
        public const string fail = "FAILED";
        #endregion
    }
}
