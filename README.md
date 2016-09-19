#Psych Exposure Therapy Project for HoloLens

From Assets and BuildSettings, the project should build successfully.

Issues that remain:

No mouse sounds. No spider sounds.

When the spatial mapping begins, the person wearing the HoloLens cannot move their head or it will break out. This could be solved by some editing of the scene order or having the spatial mapping and placement of the box done by the research assistant.

"Ice Cream" is the command to break out to the data screen.

The Bee AI script uses movement based on transform.position and not using a (rigidbody).AddForce. The rigidbody is used for the waypoints. This is effective to get the bee to move around but it is not consistent with the wander algorithm used later in the mouse and spider movement.

The bee wings also seem to be flapping at a weird speed. I'm not sure what causes this as it looks fine in the inspector. The bee color was said to be odd, but I do not know what it should look like because I am colorblind.

#A typical run would look like this:

Participant loads the app>

They airtap on their choice of Bee, Mouse or Spider>

They remain still while simulation loads spatial mesh>

A voice asks them for an inital anxiety rating>

They place the rectangular cube representing the glass box on to the spatial mesh, using an airtap>

One of the three simulations loads>

They are asked for their anxiety rating immediately and every minute. At the 30 second mark they are instructed to focus on a facet of the simulation "bee's buzzing" "color of the mouse">

At every rating, if the new value given is less than 1/4th of the highest rating given, we assume their anxiety has peaked and they move onto the next level>

instead of Level 4, stimulus message plays, thank you message plays and the data screen loads>

Participant removes hololens and hands it to research assistant
