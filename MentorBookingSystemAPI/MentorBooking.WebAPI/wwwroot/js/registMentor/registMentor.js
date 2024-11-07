const tabButtons = document.querySelectorAll('.tab-button');
        tabButtons.forEach(button => {
            button.addEventListener('click', () => {
                tabButtons.forEach(btn => btn.classList.remove('active'));
                button.classList.add('active');
            });
        });

        // Form submission and progress bar
        const nextButton = document.querySelector('.next-button');
        const progressLine = document.querySelector('.progress-line-filled');
        const steps = document.querySelectorAll('.step');
        let currentStep = 2;

        nextButton.addEventListener('click', () => {
            if (currentStep < 4) {
                currentStep++;
                updateProgress();
            }
        });

        function updateProgress() {
            const progress = ((currentStep - 1) / 3) * 100;
            progressLine.style.width = `${progress}%`;
            
            steps.forEach((step, index) => {
                if (index < currentStep) {
                    step.classList.add('active');
                } else {
                    step.classList.remove('active');
                }
            });
        }