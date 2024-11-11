// lấy số dư
const walletService = 
{
    async getBalance() {
        //await gọi get, gửi y/c -> endpoint
        const response = await apiService.get('/api/wallet/balance'); //lấy số dư hiện tại
        return response.data.balance; // chứa dữ liệu trả về từ  máy chủ
        //thuộc tính: balacnce
        //dữ liệu nhận được: data.balance
    },
// trừ điểm ví
    async deductPoints(meetingId, points) {
        await apiService.post('/api/wallet/deduct', {
            meetingId,
            points
        });
    },
//lấy ls giao dịch
    async getTransactionHistory() {
        const response = await apiService.get('/api/wallet/transactions');
        return response.data;
    }
};