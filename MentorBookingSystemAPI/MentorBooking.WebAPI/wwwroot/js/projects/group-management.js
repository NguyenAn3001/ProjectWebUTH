const projectGroup = {
    async createGroup(groupData) {
        try {
            const response = await apiService.post('/api/projects/groups', groupData);
            showSuccess('Nhóm đã được tạo thành công');
            return response.data;
        } catch (error) {
            handleError(error);
        }
    },

    async addMember(groupId, memberId) {
        try {
            await apiService.post(`/api/projects/groups/${groupId}/members`, {
                memberId: memberId
            });
            updateMemberList();
        } catch (error) {
            handleError(error);
        }
    },

    async updateProgress(groupId, progressData) {
        try {
            await apiService.put(`/api/projects/groups/${groupId}/progress`, progressData);
            updateProgressUI();
        } catch (error) {
            handleError(error);
        }
    }
};