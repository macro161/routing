
package routing;

import javax.swing.JFrame;
import javax.swing.SwingUtilities;

public class Routing {

    public static void main(String[] args) {
        
        RoutingGUI gui = new RoutingGUI();
        SwingUtilities.invokeLater(new Runnable() {
        public void run() {
            JFrame frame = new JFrame();
            frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
            frame.getContentPane().add(gui);
            frame.pack();
            frame.setVisible(true);
        }
    });

    }
    
}
