# Hello, world!
#
# This is an example function named 'hello'
# which prints 'Hello, world!'.
#
# You can learn more about package authoring with RStudio at:
#
#   http://r-pkgs.had.co.nz/
#
# Some useful keyboard shortcuts for package authoring:
#
#   Build and Reload Package:  'Ctrl + Shift + B'
#   Check Package:             'Ctrl + Shift + E'
#   Test Package:              'Ctrl + Shift + T'

hello <- function() {
  library(NLP)
  library(tm)
  library(Rstem)
  library(sentiment)
  browser();
  Emails <- read.csv("C:/Users/sachin.babladi/Desktop/Emails.csv", header=FALSE, comment.char="#")
    --View(Emails)
   mycorpus <- Corpus(VectorSource(Emails))

     mycorpus <- tm_map(mycorpus, removePunctuation)
   for(j in seq(mycorpus))
    {
       mycorpus[[j]] <- gsub("/", " ", mycorpus[[j]])
       mycorpus[[j]] <- gsub("@", " ", mycorpus[[j]])
       mycorpus[[j]] <- gsub("\\|", " ", mycorpus[[j]])
   }
   mycorpus <- tm_map(mycorpus, tolower)
   mycorpus <- tm_map(mycorpus, removeWords, stopwords("english"))
   mycorpus <- tm_map(mycorpus, stemDocument)
   mycorpus <- tm_map(mycorpus, stripWhitespace)
   mycorpus <- tm_map(mycorpus, removeNumbers)
   mycorpus <- tm_map(mycorpus, removeWords, c("exchanged", "Password Vault"))
   mycorpus <- tm_map(mycorpus, PlainTextDocument)
   dataframe<-data.frame(text=unlist(sapply(mycorpus, `[`, "content")),stringsAsFactors=F)

   write.csv(dataframe, file = "C:/Users/sachin.babladi/Desktop/MyData_2.csv")
   class_pol = classify_polarity(dataframe, algorithm="bayes")
   polarity = class_pol[,4]
}
