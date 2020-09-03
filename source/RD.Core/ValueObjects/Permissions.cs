using Headspring;

namespace RD.Core.ValueObjects {
    public class Permissions : Enumeration<Permissions> {
        public static Permissions ViewAccount = new Permissions(1, "User may view account information.");

        public static Permissions ProvisionDevices =
            new Permissions(2, "User may establish locations, and provision devices.");

        public static Permissions ManagePanRules =
            new Permissions(4, "User may manage rules for authenticating valid card PANs");

        public static Permissions LoadBalances = new Permissions(8, "User may load balances from other providers.");
        public static Permissions ExportHistory = new Permissions(16, "User may export card transaction history.");
        public static Permissions Reports = new Permissions(32, "User may access reports.");
        public static Permissions ActivateCard = new Permissions(64, "User may activate a card.");

        public static Permissions VoidLastTransaction =
            new Permissions(128, "User may void the last transaction for a card.");

        public static Permissions TransferBalance = new Permissions(256, "User may transfer balances between cards.");

        public static Permissions ManualAdjustments =
            new Permissions(512, "User may make manual adjustments to a card's value.");

        public static Permissions DeactivateACard = new Permissions(1024, "User may deactivate a card.");
        public static Permissions ManageApiClients = new Permissions(2048, "Manages processing API client tokens.");
        public static Permissions PurgeAccountData = new Permissions(4096, "User may purge account transaction data.");
        public static Permissions HasDeveloperAccess = new Permissions(8192, "User may access API documentation. [future: and request developer credentials]");

        public Permissions(int value, string displayName) : base(value, displayName) {
        }
    }
}