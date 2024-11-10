const reviewService = {
    async submitReview(meetingId, reviewData) {
        try {
            await apiService.post(`/api/reviews/meetings/${meetingId}`, reviewData);
            showSuccess('Đánh giá đã được gửi');
            updateReviewsList();
        } catch (error) {
            handleError(error);
        }
    },

    async getReviews(mentorId) {
        const response = await apiService.get(`/api/reviews/mentors/${mentorId}`);
        return response.data;
    }
};