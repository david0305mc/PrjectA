using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPubTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var pub = new Publisher();
        var sub1 = new Subscriber(pub);
        var sub2 = new Subscriber(pub);

        sub1.Publisher.Handler += delegate (object sender, Message msg)
        {
            Debug.Log($"Sub1: {msg.Content}");
        };

        sub2.Publisher.Handler += delegate (object sender, Message msg)
        {
            Debug.Log($"Sub2: {msg.Content}");
        };

        pub.Publish("hey");
    }


    class Message
    {
        public string Content { get; set; }
        public Message(string _content)
        {
            Content = _content;
        }
    }

    interface IPublisher
    {
        event EventHandler<Message> Handler;
        void Publish(string cont);
    }

    class Publisher : IPublisher
    {
        public event EventHandler<Message> Handler;

        public void OnPublish(Message msg)
        {
            Handler?.Invoke(this, msg);
        }

        public void Publish(string cont)
        {
            Message msg = (Message)Activator.CreateInstance(typeof(Message), cont);
            OnPublish(msg);
        }
    }

    class Subscriber
    {
        public IPublisher Publisher { get; set; }
        public Subscriber(IPublisher publisher)
        {
            Publisher = publisher;
        }
    }
}
