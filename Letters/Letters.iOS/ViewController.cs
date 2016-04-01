using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CoreAnimation;
using CoreGraphics;
using CoreText;
using Foundation;
using UIKit;

namespace Letters.iOS
{
	public partial class ViewController : UIViewController
	{
		int currentLetter = -1;

		double duration = 3.0;

		List<string> alphabet = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
												   "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
												   "U", "V", "W", "X", "Y", "Z" };

		CALayer animationLayer;

		CAShapeLayer pathLayer;

		protected ViewController (IntPtr handle) : base (handle) { }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			animationLayer = new CALayer ();
			animationLayer.Frame = new CGRect (0, 40.0, View.Layer.Bounds.Width, View.Layer.Bounds.Height - 84.0);
			View.Layer.AddSublayer (animationLayer);

			incrementLetter ();
		}


		void clearLayer ()
		{
			pathLayer?.RemoveFromSuperLayer ();
			pathLayer = null;
		}


		void incrementLetter ()
		{
			currentLetter++;

			if (currentLetter < alphabet.Count) {
				setupTextLayer (alphabet [currentLetter]);
				startAnimation ();
			} else if (currentLetter == alphabet.Count) {
				setupTextLayer ("All Done!");
				startAnimation ();
			} else {
				currentLetter = -1;
				clearLayer ();
			}
		}


		static nfloat letterFontSize = 380;
		static nfloat messageFontSize = 80;

		static NSMutableParagraphStyle letterParagraphStyle = new NSMutableParagraphStyle {
			LineBreakMode = UILineBreakMode.WordWrap,
			Alignment = UITextAlignment.Center
		};

		static UIStringAttributes letterStringAttributes = new UIStringAttributes {
			Font = UIFont.SystemFontOfSize (letterFontSize),
			ParagraphStyle = letterParagraphStyle
		};

		static UIStringAttributes messageStringAttributes = new UIStringAttributes {
			Font = UIFont.SystemFontOfSize (messageFontSize),
			ParagraphStyle = letterParagraphStyle
		};

		void setupTextLayer (string l)
		{
			clearLayer ();

			var singleLetter = l.Length == 1;

			var fontSize = singleLetter ? letterFontSize : messageFontSize;

			var font = new CTFont (new CTFontDescriptor (new CTFontDescriptorAttributes { Name = UIFont.SystemFontOfSize (fontSize).Name, Size = (float?)fontSize }), fontSize);

			var attrStr = new NSAttributedString (l, singleLetter ? letterStringAttributes : messageStringAttributes);

			var line = new CTLine (attrStr);

			var runArray = line.GetGlyphRuns ();

			var letters = new CGPath ();

			for (int runIndex = 0; runIndex < runArray.Length; runIndex++) {

				var run = runArray [runIndex];

				for (int runGlyphIndex = 0; runGlyphIndex < run.GlyphCount; runGlyphIndex++) {

					var thisGlyphRange = new NSRange (runGlyphIndex, 1);

					var glyph = run.GetGlyphs (thisGlyphRange).FirstOrDefault ();

					var position = run.GetPositions (thisGlyphRange).FirstOrDefault ();

					var letter = font.GetPathForGlyph (glyph);

					var t = CGAffineTransform.MakeTranslation (position.X, position.Y);

					if (letter != null) letters.AddPath (t, letter);
				}
			}

			var path = new UIBezierPath ();

			path.MoveTo (CGPoint.Empty);

			path.AppendPath (UIBezierPath.FromPath (letters));

			var layer = new CAShapeLayer ();

			layer.Frame = new CGRect (((animationLayer.Bounds.Width - path.Bounds.Width) / 2) - 10, (animationLayer.Bounds.Height - path.Bounds.Height) / 2, path.Bounds.Width, path.Bounds.Height);
			layer.GeometryFlipped = true;
			layer.Path = path.CGPath;
			layer.StrokeColor = UIColor.Blue.CGColor;
			layer.FillColor = null;
			layer.LineWidth = 3;
			layer.LineJoin = CAShapeLayer.JoinBevel;

			animationLayer?.AddSublayer (layer);

			pathLayer = layer;
		}


		void startAnimation ()
		{
			pathLayer?.RemoveAllAnimations ();

			var pathAnimation = CABasicAnimation.FromKeyPath ("strokeEnd");

			pathAnimation.AnimationStopped += pathAnimationStopped;
			pathAnimation.Duration = duration;
			pathAnimation.From = NSObject.FromObject (0);
			pathAnimation.To = NSObject.FromObject (1);

			pathLayer?.AddAnimation (pathAnimation, "strokeEnd");
		}


		async void pathAnimationStopped (object sender, CAAnimationStateEventArgs e)
		{
			var pathAnimation = sender as CABasicAnimation;

			if (pathAnimation != null) {

				pathAnimation.AnimationStopped -= pathAnimationStopped;

				pathLayer.FillColor = UIColor.Blue.CGColor;

				await Task.Delay ((int)duration * 1000);

				incrementLetter ();
			}
		}
	}
}