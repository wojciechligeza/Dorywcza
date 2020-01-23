namespace Dorywcza.Services.EmailService.EmailText
{
    public static class EmailText
    {
        public const string EmailToEmployee = "<h3>Formularz oferty pracy</h3><br><p>" +
                                              "Szanowny Kliencie, <br>" +
                                              "Twój formularz został wysłany." +
                                              "Proszę czekać na wiadomość od pracodawcy.</p><br>" +
                                              "<h4>Z wyrazami szacunku,<h4><br>" +
                                              "Zespół <i>Dorywcza.pl</i>";

        public const string EmailBackYesToEmployee = "<h3>Gratulacje</h3><br>" +
                                                     "<p>Szanowny Kliencie, <br>" +
                                                     "Pracodawca przyjął twoją prośbę." +
                                                     "Teraz tylko stawić się na umówione miejsce o umówionym czasie i patrzeć jak kasa przypływa na konto :)</p><br>" +
                                                     "<h4>Z wyrazami szacunku,<h4><br>" +
                                                     "Zespół <i>Dorywcza.pl</i>";

        public const string EmailBackNoToEmployee = "<h3>Prośba o oferte pracy</h3><br>" +
                                                    "<p>Szanowny Kliencie, <br>Niestety pracodawca odrzucił twoją prośbę. " +
                                                    "Ale nie przejmuj się, są jeszcze inne oferty pracy ;)</p><br>" +
                                                    "<h4>Z wyrazami szacunku,<h4><br>" +
                                                    "Zespół <i>Dorywcza.pl</i>";
    }
}
