namespace Bookshop.Utility
{
    internal class SendGridClient
    {
        private object sendGridSecret;

        public SendGridClient(object sendGridSecret)
        {
            this.sendGridSecret = sendGridSecret;
        }
    }
}