const notificationService = {
    initializeWebSocket() {
        const ws = new WebSocket('wss://your-domain/notifications');
        ws.onmessage = this.handleNotification;
    },

    handleNotification(event) {
        const notification = JSON.parse(event.data);
        this.showNotification(notification);
    },

    showNotification(notification) {
        // Show notification using browser notification API
        if (Notification.permission === 'granted') {
            new Notification(notification.title, {
                body: notification.message
            });
        }
    },

    async getNotificationSettings() {
        return await apiService.get('/api/notifications/settings');
    },

    async updateSettings(settings) {
        await apiService.put('/api/notifications/settings', settings);
    }
};