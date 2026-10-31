So, here’s the scoop! 🎉 I’ve been collecting all my favorite quotes – you know, the ones that really hit home – and popping them into my MongoDB database. Then I thought, why keep all the good vibes to myself?

Enter Project HypeMan (okay, might change the name, but it sounds cool). Using Twilio’s API magic, I’ve set up a program that grabs a random quote from my database, fetches a list of contacts also from my database, and poof! sends that quote out to everyone! Scheduled it to run like clockwork on my server so every day, everyone on my list gets a fresh dose of wisdom at 4pm. Why 4pm? Don't ask me that. It's end of day almost, you need that pump to do better and I got you.

Twilio has changed the free accounts policy, No free texts as of now. Will be looking for any other free alternatives.

A little daily inspiration, coming right up! 🌟

## Run with Docker and cron

The container runs the app once and exits. Schedule it from the host with cron;
the example below runs every day at 4:00 PM UTC.

1. Make sure `.env` contains `MONGO_USERNAME`, `MONGO_PASSWORD`, `ACCOUNT_SID`,
   and `AUTH_TOKEN`. Keep this file private; it is excluded from the Docker image.
2. From the repository directory, build the image:

   ```sh
   docker build -t hypeman:latest .
   ```

3. Add a cron entry with `crontab -e`, replacing the paths with absolute paths
   on the host. On Linux cron implementations that support `CRON_TZ`, use:

   ```cron
   CRON_TZ=UTC
   0 16 * * * /usr/bin/docker run --rm --env-file /absolute/path/to/HypeMan/.env hypeman:latest >> /absolute/path/to/HypeMan/hypeman-cron.log 2>&1
   ```

   Find the Docker executable path with `command -v docker`. If your cron
   implementation does not support `CRON_TZ`, configure the host's cron
   timezone as UTC so the job runs at 4:00 PM UTC.

The host must keep its Docker daemon running, and the cron user must be able to
access it. Rebuild the image after changing the application.
