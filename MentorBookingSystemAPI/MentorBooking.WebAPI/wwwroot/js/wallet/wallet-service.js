// js/wallet/wallet-service.js
const walletService = {
    async getBalance() {
        const response = await apiService.get('/api/wallet/balance');
        return response.data.balance;
    },

    async deductPoints(meetingId, points) {
        await apiService.post('/api/wallet/deduct', {
            meetingId,
            points
        });
    },

    async getTransactionHistory() {
        const response = await apiService.get('/api/wallet/transactions');
        return response.data;
    }
};