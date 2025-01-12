FROM alpine:latest

RUN echo "This is a custom test image for https://github.com/echokrist/DCEUI"

# Add labels to describe the image
LABEL maintainer="echokrist <echokrist@pm.me>"
LABEL org.opencontainers.image.title="DCEUI Custom Test Container"
LABEL org.opencontainers.image.description="A unique test container for the DCEUI project, specifically designed for Docker-related tests."
LABEL org.opencontainers.image.url="https://github.com/echokrist/DCEUI"
LABEL org.opencontainers.image.source="https://github.com/echokrist/DCEUI"
LABEL org.opencontainers.image.version="1.0.0"
LABEL org.opencontainers.image.licenses="MIT"
LABEL com.echokrist.dceui.test="true"

VOLUME /DCEUI

# Default command to display the test information
CMD ["tail", "-f", "/dev/null"]
