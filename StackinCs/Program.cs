using System;
using System.Collections;
using System.Collections.Generic;


namespace Linkedlist
{
    public class Document
    {
        static void Main()
        {
            PriorityDocumentManager pdm = new PriorityDocumentManager();

            pdm.AddDocument(new Document(" First doc", "Sample", 8));
            pdm.AddDocument(new Document(" Two doc", "Sample", 3));
            pdm.AddDocument(new Document(" Three doc", "Sample", 4));
            pdm.AddDocument(new Document(" Four doc", "Sample", 8));
            pdm.AddDocument(new Document(" Five doc", "Sample", 1));
            pdm.AddDocument(new Document(" six doc", "Sample", 9));
            pdm.AddDocument(new Document(" seven doc", "Sample", 1));
            pdm.AddDocument(new Document(" eight doc", "Sample", 1));
            pdm.DisplayAllNodes();
        }

        private string title;
        public string Title
        {
            get { return title; }
        }

        private string content;
        public string Content
        {
            get { return content; }
        }
        private byte priority;
        public byte Priority
        { get { return priority; } }

        public Document (string title, string content, byte priority)
        {
            this.title = title;
            this.content = content;
            this.priority = priority;
        }
    }

    public class PriorityDocumentManager
    {
        private readonly LinkedList<Document> documentList;

        //priorities 0...9
        private readonly List<LinkedListNode<Document>> priorityNodes;

        public PriorityDocumentManager()
        {
            documentList = new LinkedList<Document>();

            priorityNodes = new List<LinkedListNode<Document>>(10);
            for (int i = 0; i < 10; i++)
            {
                priorityNodes.Add(new LinkedListNode<Document>(null));
            }
        }

        public void AddDocument(Document d)
        {
            if (d == null)
                throw new ArgumentNullException("d");
            AddDocumentToPriorityNode(d, d.Priority);
        }

        private void AddDocumentToPriorityNode(Document doc, int priority)
        {
            if (priority > 9 || priority < 0)
                throw new ArgumentException("Priority must be between 0 to 9");

            if (priorityNodes[priority].Value == null)
            {
                priority--;
                if (priority >= 0)
                {
                    //chech for the next lower priority
                    AddDocumentToPriorityNode(doc, priority);
                }
                else
                {
                    //Now no priority node exists with the same priority or lower add the new doc to the end
                    documentList.AddLast(doc);
                    priorityNodes[doc.Priority] = documentList.Last;
                }
                return;
            }
            else // a priority node exists
            {
                LinkedListNode<Document> prioNode = priorityNodes[priority];
                if (priority == doc.Priority) //priority node with the same priority exists
                {
                    documentList.AddAfter(prioNode, doc); //set the priority node to the last document with the same priority
                    priorityNodes[doc.Priority] = prioNode.Next;
                }
                else // Only priority node with a lower priority exists
                {
                    //Get the first node of the lower priority
                    LinkedListNode<Document> firstPrioNode = prioNode;

                    while (firstPrioNode.Previous != null && firstPrioNode.Previous.Value.Priority == prioNode.Value.Priority)
                    {
                        firstPrioNode = prioNode.Previous;
                    }

                    documentList.AddBefore(firstPrioNode, doc);
                    //set the priority node to the new value
                    priorityNodes[doc.Priority] = firstPrioNode.Previous;
                }
            }
        }
        public void DisplayAllNodes()
        {
            foreach (Document doc in documentList)
            {
                Console.WriteLine("priority: {0}, title{1}", doc.Priority, doc.Title);
            }
        }
        //returns the document with the higest priority
        //Thats first inn the linked list
        public Document GetDocument()
        {
            Document doc = documentList.First.Value;
            documentList.RemoveFirst();
            return doc;
        }
    }
}