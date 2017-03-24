using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AssignmentComplete
{
	class Mine : IFactory
	{

		class AddOreBoxToMine : IAction
		{
			Mine mine;
			public AddOreBoxToMine(Mine mine) //constructor
			{
				this.mine = mine;
			}
			public void Run()
			{
				mine.ProductsToShip.Add(CreateOreBox(mine.Position + new Vector2(-80, 40 + -30 * mine.ProductsToShip.Count)));
			}
			Ore CreateOreBox(Vector2 position) //protected
			{
				var box = new Ore(100, mine.oreBox);
				box.Position = position;
				return box;
			}
		}

		Texture2D mine, oreContainer, oreBox, truckTexture;
		List<IStateMachine> processes;
		ITruck waitingTruck;
		bool isTruckReady = false;
		Vector2 position;
		List<IContainer> productsToShip;

		public Mine(Vector2 position, Texture2D truck_texture, Texture2D mine, Texture2D ore_box, Texture2D ore_container)
		{
			processes = new List<IStateMachine>();
			ProductsToShip = new List<IContainer>();
			this.mine = mine;
			this.truckTexture = truck_texture;
			this.oreContainer = ore_container;
			this.oreBox = ore_box;
			this.position = position;


			processes.Add( //statemachine
			  new Repeat(new Seq(new Timer(50.0f),
								 new Call(new AddOreBoxToMine(this)))));

			processes.Add(new Repeat(new Seq(new Wait(() => productsToShip.Count() == 5), new Call(new AddContainer(this)))));

			processes.Add(new Repeat(new Seq(new Wait(() => waitingTruck == null), new Call(new AddTruck(this)))));
		}
		class AddTruck : IAction
		{
			Mine mine;
			public AddTruck(Mine mine)
			{
				this.mine = mine;
			}

			public void Run()
			{
				mine.waitingTruck = new Truck(mine.position, new Vector2(0, 0), mine.truckTexture, true);				
			}
		}

		class AddContainer : IAction
		{
			Mine mine;
			public AddContainer(Mine mine)
			{
				this.mine = mine;
			}

			public void Run()
			{
				mine.waitingTruck.AddContainer(new OreContainer(5, mine.oreContainer));
				mine.isTruckReady = true;
				mine.productsToShip.Clear();
			}
		}


		public ITruck GetReadyTruck()//instantie van class truck teruggeven
		{
			if (isTruckReady)
			{
				ITruck atruck = waitingTruck;
				waitingTruck = null;
				isTruckReady = false;
				return atruck;
			}
			else
			{
				return null;
			}
		}

    public Vector2 Position
    {
      get
      {
        return position;
      }
    }
    public List<IContainer> ProductsToShip
    {
      get
      {
        return productsToShip;
      }
      set
      {
        productsToShip = value;
      }
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (var cart in productsToShip)
      {
        cart.Draw(spriteBatch);
      }
      spriteBatch.Draw(mine, Position, Color.White);

      if(waitingTruck != null)
			{
				waitingTruck.Draw(spriteBatch);
			}
    }
    public void Update(float dt)
    {
	  if (productsToShip.Count() == 5){
				isTruckReady = true;
			}
	 	//we vergaten .Count() en het moest in de update class.
      foreach (var process in processes)
      {
        process.Update(1);
      }
    }
    
  }

	class Ikea : IFactory
	{

		class AddProductstoIkea : IAction
		{
			Ikea ikea;
			public AddProductstoIkea(Ikea ikea) //constructor
			{
				this.ikea = ikea;
			}
			public void Run()
			{
				ikea.ProductsToShip.Add(CreateProductBox(ikea.Position + new Vector2(120, 20 + -30 * ikea.ProductsToShip.Count)));
			}
			Product CreateProductBox(Vector2 position) //protected
			{
				var box = new Product(100, ikea.productBox);
				box.Position = position;
				return box;
			}
		}

		Texture2D ikea, productContainer, productBox, truckTexture;
		List<IStateMachine> processes;
		ITruck waitingTruck;
		bool isTruckReady = false;
		Vector2 position;
		List<IContainer> productsToShip;

		public Ikea(Vector2 position, Texture2D truck_texture, Texture2D ikea, Texture2D product_box, Texture2D product_container)
		{
			processes = new List<IStateMachine>();
			ProductsToShip = new List<IContainer>();
			this.ikea = ikea;
			this.truckTexture = truck_texture;
			this.productContainer = product_container;
			this.productBox = product_box;
			this.position = position;
            this.waitingTruck = new Truck(this.position, new Vector2(0, 0), this.truckTexture, false);


			processes.Add( //statemachine
			  new Repeat(new Seq(new Timer(50.0f),
								 new Call(new AddProductstoIkea(this)))));

			processes.Add(new Repeat(new Seq(new Wait(() => productsToShip.Count() == 5), new Call(new AddContainer(this)))));

			processes.Add(new Repeat(new Seq(new Wait(() => waitingTruck == null), new Call(new AddTruck(this)))));
		}
		class AddTruck : IAction
		{
			Ikea ikea;
			public AddTruck(Ikea ikea)
			{
				this.ikea = ikea;
			}

			public void Run()
			{
				ikea.waitingTruck = new Truck(ikea.position, new Vector2(0, 0), ikea.truckTexture, false);
			}
		}

		class AddContainer : IAction
		{
			Ikea ikea;
			public AddContainer(Ikea ikea)
			{
				this.ikea = ikea;
			}

			public void Run()
			{
				ikea.waitingTruck.AddContainer(new OreContainer(5, ikea.productContainer));
                ikea.productsToShip.Clear();
                ikea.isTruckReady = true;
			}
		}


		public ITruck GetReadyTruck()//instantie van class truck teruggeven
		{
			if (isTruckReady)
			{
				ITruck atruck = waitingTruck;
				waitingTruck = null;
				isTruckReady = false;
				return atruck;
			}
			else
			{
				return null;
			}
		}

		public Vector2 Position
		{
			get
			{
				return position;
			}
		}
		public List<IContainer> ProductsToShip
		{
			get
			{
				return productsToShip;
			}
			set
			{
				productsToShip = value;
			}
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var cart in productsToShip)
			{
				cart.Draw(spriteBatch); //mijnkarretjes?
			}
			spriteBatch.Draw(ikea, Position, Color.White);

			if (waitingTruck != null)
			{
				waitingTruck.Draw(spriteBatch);
			}
		}
		public void Update(float dt)
		{
            if (productsToShip.Count() == 5)
			{
				isTruckReady = true;
			}
			//we vergaten .Count() en het moest in de update class.
			foreach (var process in processes)
			{
				process.Update(1);
			}
		}

	}
}
