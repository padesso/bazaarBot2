﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EconomySim
{
    public class TradeBook
    {
	    public Dictionary<string, List<Offer>> bids;
	    public Dictionary<string, List<Offer>> asks;

        //public DataTable dbook { get; set; }

	    public TradeBook()
	    {
		    bids = new Dictionary<string, List<Offer>>();
		    asks = new Dictionary<string, List<Offer>>();
            //dbook = new DataTable("Book");
            //dbook.Columns.Add(new DataColumn("bid"));
            //dbook.Columns.Add(new DataColumn("ask"));
            //dbook.Rows.Add(1.0,2.0);
        }

	    public void Register(string name)
	    {
		    asks[name] = new List<Offer>();
		    bids[name] = new List<Offer>();
	    }

	    public bool Bid(Offer offer)
	    {
		    if (!bids.ContainsKey(offer.commodity))
			    return false;

		    bids[offer.commodity].Add(offer);
		    return true;
	    }

	    public bool Ask(Offer offer)
	    {
		    if (!bids.ContainsKey(offer.commodity))
			    return false;

		    asks[offer.commodity].Add(offer);
		    return true;
	    }
    }
}
