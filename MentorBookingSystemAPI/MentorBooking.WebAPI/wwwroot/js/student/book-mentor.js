async function bookMentor() {
    const token = getDynamicToken(); // Get the dynamic token
  
    if (!token) {
      alert("Token not found! Please login.");
      return;
    }
  
    const bookingData = getDynamicBookingData(); // Get dynamic data
  
    // Log the data to ensure it's correct before sending the request
    console.log('Booking data:', bookingData);
  
    try {
      const response = await fetch('http://localhost:5076/api/BookingMentor/booking-mentor', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
          'Accept': '*/*'
        },
        body: JSON.stringify(bookingData)
      });
  
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
  
      const responseData = await response.json();
      console.log('Response data:', responseData);
  
      if (responseData.status === 'Success') {
        alert('Booking is successful!');
      } else {
        alert('Booking failed. Message: ' + responseData.message);
      }
    } catch (error) {
      console.error('Error occurred while booking mentor:', error);
    }
  }
  